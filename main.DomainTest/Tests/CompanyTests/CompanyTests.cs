using Main.Domain.CompanyDomain;

namespace main.DomainTest.Tests.CompanyTests
{
    public class CompanyCreate
    {
        private readonly IFixture _fixture;

        public CompanyCreate()
        {
            _fixture = new Fixture();
        }

        public static IEnumerable<Object[]> GetNotValidTestsData()
        {
            yield return new object[]
            {
                "",
                "Valid Description",
                "Наименование компании не может быть пустым"
            };

            yield return new object[]
            {
                "Valid Name",
                "",
                "Описание компании не может быть пустым"
            };

            yield return new object[]
            {
                "test",
                "Valid Description",
                $"Длина наименования компании не может быть меньше {Company.MinLengthName}"
            };
        }

        [Fact]
        public void Create_Company_With_Valid_Data()
        {
            var validName = _fixture.Create<string>();
            var validDescription = _fixture.Create<string>();

            var resultCreate = Company.Create(validName, validDescription);
            var company = resultCreate.Value;

            Assert.True(resultCreate.IsSuccess);
            Assert.NotNull(company);
            Assert.Equal(validName, company.Name);
            Assert.Equal(validDescription, company.Description);
        }

        [Theory]
        [MemberData(nameof(GetNotValidTestsData))]
        public void Create_Company_With_NotValid_Data(string name, string description, string errorMessage)
        {
            var resultCreate = Company.Create(name, description);

            Assert.True(resultCreate.IsFailure);
            Assert.Null(resultCreate.Value);
            Assert.Equal(resultCreate.Error, errorMessage);
        }
    }
}
