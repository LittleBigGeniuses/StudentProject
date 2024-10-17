using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class WorkflowCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Workflow>(composer =>
                composer.FromFactory(() =>
                {
                    var validTemplate = fixture.Create<WorkflowTemplate>();

                    var validAuthorId = fixture.Create<Guid>();
                    var validCandidateId = fixture.Create<Guid>();

                    var result = Workflow.Create(validAuthorId, validCandidateId, validTemplate);

                    if (result.IsFailure)
                    {
                        throw new InvalidOperationException($"Failed to create Workflow: {result.Error}");
                    }

                    var workflow = result.Value!;

                    return workflow;
                }));
        }
    }
}
