using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace main.DomainTest.Customizations
{
    /// <summary>
    /// Расширение для подключения кастомизаций
    /// </summary>
    public static class FixtureCustomizationExtension
    {
        public static void FixtureCustomization(this IFixture fixture)
        {
            fixture.Customize(new WorkflowCustomization());
            fixture.Customize(new EmployeeCustomization());
            fixture.Customize(new WorkflowTemplateWithStepsCustomization());
        }
    }
}
