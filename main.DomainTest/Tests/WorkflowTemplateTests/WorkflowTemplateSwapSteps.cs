using Main.Domain.WorkflowTemplateDomain;
using main.DomainTest.TestTools.Autofixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.DomainTest.Tests.WorkflowTemplateTests
{
    public class WorkflowTemplateSwapSteps
    {
        private readonly IFixture _fixture;
        private readonly WorkflowTemplate _workflowTemplate;


        public WorkflowTemplateSwapSteps()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflowTemplate = _fixture.Create<WorkflowTemplate>();
        } 

        /// <summary>
        /// Тестирование обмена шагов при валидных данных 
        /// </summary>
        [Fact]
        public void SwapSteps_WorkflowTemplate_With_Valid_Data()
        {
            var numberFirst = _workflowTemplate.Steps.First().Number;
            var numberSecond = _workflowTemplate.Steps.Last().Number;
            var curNumber = 1;

            var result = _workflowTemplate.SwapSteps(numberFirst, numberSecond);

            Assert.True(result.IsSuccess);
            foreach (var step in _workflowTemplate.Steps)
            {
                Assert.Equal(curNumber, step.Number);
                curNumber++;
            }
        }

        /// <summary>
        /// тест с набором невалидных данных
        /// </summary>
        /// <param name="expectedErrorMessage"></param>
        [Fact]
        public void SwapSteps_ShouldReturnFailure_WhenInvalidInput()
        {
            var numberFirst = _workflowTemplate.Steps.First().Number - 1;
            var numberSecond = _workflowTemplate.Steps.Last().Number + 1;

            var result = _workflowTemplate.SwapSteps(numberFirst, numberSecond);

            Assert.True(result.IsFailure);
            Assert.Equal("Шаблон не содержит шаг с таким номером", result.Error);
        }
    }
}

