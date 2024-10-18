using Main.Domain.CompanyDomain;

namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class CompanyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Company>(composer =>
                composer.FromFactory(() =>
                {
                    var validName = fixture.Create<string>().Substring(0, Company.MinLengthName + 1);
                    var validDescription = fixture.Create<string>();

                    var employee = Company.Create(validName, validDescription).Value!;

                    return employee;
                }));
        }
    }
}
