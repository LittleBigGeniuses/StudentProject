using Main.Domain.CandidateDomain;

namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class CandidateCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Candidate>(composer =>
                composer.FromFactory(() =>
                {
                    var validName = fixture.Create<string>().Substring(0, Candidate.MinLengthName + 1);
                    var validDescription = fixture.Create<string>();

                    var candidate = Candidate.Create(validName).Value!;

                    return candidate;
                }));
        }
    }
}
