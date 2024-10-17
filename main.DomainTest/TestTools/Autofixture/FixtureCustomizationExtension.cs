using AutoFixture;
using main.DomainTest.Customizations;
using main.DomainTest.TestTools.Autofixture.Customizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
