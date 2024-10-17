using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class WorkflowStepTemplateCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<WorkflowStepTemplate>(composer =>
                composer.FromFactory(() =>
                {
                    var validNumber = fixture.Create<int>();
                    var validDescription = fixture.Create<string>();
                    var validEmployeeId = fixture.Create<Guid?>();
                    var validRoleId = fixture.Create<Guid?>();

                    var result = WorkflowStepTemplate.Create(validNumber, validDescription, validEmployeeId, validRoleId);

                    if (result.IsFailure)
                    {
                        throw new InvalidOperationException($"Failed to create WorkflowStepTemplate: {result.Error}");
                    }

                    return result.Value!;
                }));
        }
    }
}
