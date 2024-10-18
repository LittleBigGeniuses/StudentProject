using Main.Domain.CompanyDomain;

namespace main.DomainTest.Tests.CompanyTests
{
    public class CompanyCtorTest
    {
        public class CompanyCtor : Company
        {
            public CompanyCtor(
                Guid id,
                string name,
                string description,
                DateTime dateCreate,
                DateTime dateUpdate)
                : base(id, name, description, dateCreate, dateUpdate)
            { }
        }

        private readonly IFixture _fixture;

        public CompanyCtorTest()
        {
            _fixture = new Fixture();
        }

        public static IEnumerable<object[]> GetUnValidData()
        {
            yield return new object[]
            {
                Guid.Empty,
                "Valid Name",
                "Valid description",
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор Компании"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "",
                "Valid description",
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "Наименование компании не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "",
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "Описание компании не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid description",
                DateTime.MinValue,
                DateTime.UtcNow,
                typeof(ArgumentException),
                "Дата создания не может быть дефолтной."
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid description",
                DateTime.UtcNow,
                DateTime.MinValue,
                typeof(ArgumentException),
                "Дата обновления не может быть дефолтной."
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "test",
                "Valid description",
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentException),
                $"Длина наименования компании не может быть меньше {Company.MinLengthName}"
            };

        }

        [Fact]
        public void Company_Constructor_With_Valid_Data()
        {
            Guid id = _fixture.Create<Guid>();
            string name = _fixture.Create<string>();
            string description = _fixture.Create<string>();
            DateTime dateCreate = DateTime.UtcNow;
            DateTime dateUpdate = DateTime.UtcNow;

            var company = new CompanyCtor(
                id,
                name,
                description,
                dateCreate,
                dateUpdate);

            Assert.NotNull(company);
            Assert.Equal(company.Id, id);
            Assert.Equal(company.Name, name);
            Assert.Equal(company.Description, description);
            Assert.Equal(company.DateCreate, dateCreate);
            Assert.Equal(company.DateUpdate, dateUpdate);
        }

        [Theory]
        [MemberData(nameof(GetUnValidData))]
        public void Company_Constructor_With_UnValid_Data(
            Guid id,
            string name,
            string description,
            DateTime dateCreate,
            DateTime dateUpdate,
            Type expectedExceptionType,
            string exeptionMessage)
        {
            var exception = Assert.Throws(expectedExceptionType, () => 
                new CompanyCtor(
                    id,
                    name,
                    description,
                    dateCreate,
                    dateUpdate));

            Assert.Contains(exeptionMessage, exception.Message);
        }

    }
}
