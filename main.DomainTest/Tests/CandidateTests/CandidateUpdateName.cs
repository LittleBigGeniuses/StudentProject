using main.DomainTest.TestTools.Autofixture;
using Main.Domain.CandidateDomain;
using Main.Domain.EmployeeDomain;
using System.Data;
namespace main.DomainTest.Tests.CandidateTests

{
    public class CandidateUpdateName
    {
        private readonly IFixture _fixture;
        private Candidate _candidate;

        public CandidateUpdateName()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _candidate = _fixture.Create<Candidate>();
        }

        public static IEnumerable<object[]> GetInvalidData()
        {
            yield return new object[]
            {
                "",
                "ФИО сотрудника не может быть пустым"
            };

            yield return new object[]
            {
                "1234",
                $"Длина ФИО соискателя не может быть меньше {Candidate.MinLengthName}"
            };
        }
        [Fact]
        public void Success_UpdateName_Candidate_With_Valid_Data()
        {
            var dateUpdate = _candidate.DateUpdate;
            var lastName = _candidate.Name;
            var validName = _fixture.Create<string>().Substring(0, Candidate.MinLengthName + 1);

            var resultUpdate = _candidate.UpdateName(validName);

            Assert.True(resultUpdate.IsSuccess);
            Assert.NotEqual(_candidate.Name, lastName);
            Assert.Equal(_candidate.Name, validName);
            Assert.True(dateUpdate < _candidate.DateUpdate);
        }

        [Fact]
        public void Success_UpdateName_Candidate_With_Same_Data()
        {
            var dateUpdate = _candidate.DateUpdate;
            var lastName = _candidate.Name;

            var resultUpdate = _candidate.UpdateName(_candidate.Name);

            Assert.True(resultUpdate.IsSuccess);
            Assert.Equal(_candidate.Name, lastName);
            Assert.False(dateUpdate < _candidate.DateUpdate);
        }

        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void Failed_UpdateName_Candidate_MethodResult_With_Invalid_Data(
            string name,
            string expectError)
        {
            var dateUpdate = _candidate.DateUpdate;
            var lastName = _candidate.Name;
            var resultUpdate = _candidate.UpdateName(name);

            Assert.True(resultUpdate.IsFailure);
            Assert.Equal(resultUpdate.Error, expectError);
            Assert.Equal(lastName, _candidate.Name);
            Assert.NotEqual(name, _candidate.Name);
            Assert.False(dateUpdate < _candidate.DateUpdate);
        }
    }
}
