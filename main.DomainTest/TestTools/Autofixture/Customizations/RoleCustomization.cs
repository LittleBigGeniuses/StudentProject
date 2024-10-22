using Main.Domain.EmployeeDomain;

namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class RoleCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Role>(composer =>
            composer.FromFactory(() =>
            {
                var validName = fixture.Create<string>().Substring(0, Role.MinLengthName + 1);
                var validCompanyId = fixture.Create<Guid>();

                var role = Role.Create(validName, validCompanyId).Value!;

                return role;
            }));
        }

    }
}
