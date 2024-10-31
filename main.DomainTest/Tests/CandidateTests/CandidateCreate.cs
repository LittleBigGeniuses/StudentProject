using Main.Domain.CandidateDomain;

namespace main.DomainTest.Tests.CandidateTests
{
    public class CandidateCreate
    {
        private readonly IFixture _fixture;

        public CandidateCreate()
        {
            _fixture = new Fixture();
        }

        public static IEnumerable<object[]> GetInvalidData()
        {
            yield return new object[]
            {
                "",
                "ФИО соискателя не может быть пустым"
            };

            yield return new object[]
            {
                "1234",
                $"Длина ФИО соискателя не может быть меньше {Candidate.MinLengthName}"
            };
        }
            [Fact]
            public void Success_Role_Create_StaticMethod_With_Valid_Data()
            {
                var name = _fixture.Create<string>().Substring(0, Candidate.MinLengthName + 1);
                var resultCreateCandidate = Candidate.Create(name);
                var candidate = resultCreateCandidate.Value;

                Assert.True(resultCreateCandidate.IsSuccess);
                Assert.NotNull(candidate);
                Assert.Equal(name, candidate.Name);
            }

        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void Failed_Role_Create_With_Invalid_Data(
            string name,
            string expectedError)
        {
            var resultCreateCandidate = Candidate.Create(name);
            var role = resultCreateCandidate.Value;

            Assert.True(resultCreateCandidate.IsFailure);
            Assert.False(resultCreateCandidate.IsSuccess);
            Assert.Null(role);
            Assert.Equal(resultCreateCandidate.Error, expectedError);
        }
    }
}
