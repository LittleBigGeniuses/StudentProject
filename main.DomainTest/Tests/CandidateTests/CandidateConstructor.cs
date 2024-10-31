using Main.Domain.CandidateDomain;
using Main.Domain.EmployeeDomain;
using static main.DomainTest.Tests.RoleTests.RoleConstructor;

namespace main.DomainTest.Tests.CandidateTests
{
    public class CandidateConstuctor
    {
        public class CandidateCtor : Candidate
        {
            public CandidateCtor(
                Guid id,
                string name,
                DateTime dateCreate,
                DateTime dateUpdate)
                : base(id, name, dateCreate, dateUpdate)
            {}
        }
        private readonly IFixture _fixture;
        public CandidateConstuctor()
        {
            _fixture = new Fixture();
        }
        public static IEnumerable<object[]> GetUnvalidData()
        {
            yield return new object[]
            {
                Guid.Empty,
                "Valid Name",
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор Кандидата"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "",
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "ФИО соискателя не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                DateTime.MinValue,
                DateTime.UtcNow,
                typeof(ArgumentException),
                "Дата создания не может быть дефолтной"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                DateTime.UtcNow,
                DateTime.MinValue,
                typeof(ArgumentException),
                "Дата обновления не может быть дефолтной"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "12",
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentException),
                $"Длина ФИО соискателя не может быть меньше {Candidate.MinLengthName}"
            };
        }

            [Fact]
            public void Success_Ctor_Create_With_Valid_Data()
            {
                var id = _fixture.Create<Guid>();
                var name = _fixture.Create<string>();
                var dateCreate = _fixture.Create<DateTime>();
                var dateUpdate = _fixture.Create<DateTime>();

                var candidate = new CandidateCtor(
                    id,
                    name,
                    dateCreate,
                    dateUpdate);

                Assert.NotNull(candidate);
                Assert.Equal(id, candidate.Id);
                Assert.Equal(name, candidate.Name);
                Assert.Equal(dateCreate, candidate.DateCreate);
                Assert.Equal(dateUpdate, candidate.DateUpdate);
            }

        [Theory]
        [MemberData(nameof(GetUnvalidData))]
        public void Failed_Ctor_Create_With_UnvalidData(
                Guid id,
                string name,
                DateTime dateCreate,
                DateTime dateUpdate,
                Type exeptionType,
                string exeptionMessage)
        {
            var exeption = Assert.Throws(exeptionType, () =>
                new CandidateCtor(
                    id,
                    name,
                    dateCreate,
                    dateUpdate));

            Assert.NotNull(exeption);
            Assert.Contains(exeptionMessage, exeption.Message);
        }
    }
}
