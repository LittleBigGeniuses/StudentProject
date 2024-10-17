using Main.Domain.WorkflowTemplateDomain;


namespace main.DomainTest.TestTools.Autofixture.Customizations
{
    public class WorkflowTemplateWithStepsCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<WorkflowTemplate>(composer =>
                composer.FromFactory(() =>
                {
                    var validName = fixture.Create<string>().Substring(0, WorkflowTemplate.MinLengthName + 1);
                    var validDescription = fixture.Create<string>();
                    var validCompanyId = fixture.Create<Guid>();

                    var resultTemplateCreate = WorkflowTemplate.Create(validName, validDescription, validCompanyId);
                    var validTemplate = resultTemplateCreate.Value;

                    for (int i = 0; i < 3; i++)
                    {
                        validTemplate!.AddStep(fixture.Create<string>(), fixture.Create<Guid?>(), fixture.Create<Guid?>());
                    }

                    return validTemplate!;
                }));
        }
    }
}
