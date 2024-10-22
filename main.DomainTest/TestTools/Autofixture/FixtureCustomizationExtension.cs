using main.DomainTest.TestTools.Autofixture.Customizations;

namespace main.DomainTest.TestTools.Autofixture
{
    /// <summary>
    /// Расширение для подключения кастомизаций
    /// </summary>
    public static class FixtureCustomizationExtension
    {
        public static void FixtureCustomization(this IFixture fixture)
        {
            fixture.Customize(new WorkflowStepTemplateCustomization());
            fixture.Customize(new WorkflowTemplateWithStepsCustomization());
            fixture.Customize(new EmployeeCustomization());
            fixture.Customize(new WorkflowStepCustomization());
            fixture.Customize(new WorkflowCustomization());
            fixture.Customize(new CompanyCustomization());
        }
    }
}
