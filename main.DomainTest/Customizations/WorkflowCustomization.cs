using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.Customizations
{
    public class WorkflowCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Workflow>(composer =>
                composer.FromFactory(() =>
                {
                    fixture.Customize(new WorkflowTemplateWithStepsCustomization());
                    var validTemplate = fixture.Create<WorkflowTemplate>();

                    var validAuthorId = fixture.Create<Guid>();
                    var validCandidateId = fixture.Create<Guid>();

                    var workflow = Workflow.Create(validAuthorId, validCandidateId, validTemplate).Value;

                    return workflow!;
                }));
        }
    }
}
