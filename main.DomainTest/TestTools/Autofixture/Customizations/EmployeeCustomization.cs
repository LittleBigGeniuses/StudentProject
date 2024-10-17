using Main.Domain.EmployeeDomain;

namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class EmployeeCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Employee>(composer =>
                composer.FromFactory(() =>
                {
                    var validName = fixture.Create<string>().Substring(0, Employee.MinLengthName + 1);
                    var validCompanyId = fixture.Create<Guid>();
                    var validRoleId = fixture.Create<Guid>();

                    var employee = Employee.Create(validName, validCompanyId, validRoleId).Value!;

                    return employee;
                }));
        }
    }
}
