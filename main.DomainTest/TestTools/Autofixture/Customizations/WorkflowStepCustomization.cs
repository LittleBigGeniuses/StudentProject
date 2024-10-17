using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class WorkflowStepCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<WorkflowStep>(composer =>
                composer.FromFactory(() =>
                {
                    var validCandidateId = fixture.Create<Guid>();
                    var validStepTemplate = fixture.Create<WorkflowStepTemplate>();

                    var result = WorkflowStep.Create(validCandidateId, validStepTemplate);

                    if (result.IsFailure)
                    {
                        throw new InvalidOperationException($"Failed to create WorkflowStep: {result.Error}");
                    }

                    return result.Value!;
                }));
        }
    }
}
