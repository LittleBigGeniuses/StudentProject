using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowTemplateDomain;
using Xunit.Sdk;

namespace main.DomainTest.Tests.WorkflowTemplateTests
{
    public class WorkflowTemplateRemoveStep
    {
        private readonly IFixture _fixture;
        private readonly WorkflowTemplate _workflowTemplate;


        public WorkflowTemplateRemoveStep()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflowTemplate = _fixture.Create<WorkflowTemplate>();

        }

        [Fact]
        public void RemoveStep_WorkflowTemplate_WithValidData()
        {
            var number = 2;
            var deletedStep = _workflowTemplate.Steps.Where(s => s.Number == 2).First();
            var beginningCount = _workflowTemplate.Steps.Count;
            var curNumber = 1;


            var result = _workflowTemplate.RemoveStep(number);

            Assert.True(result.IsSuccess);
            Assert.DoesNotContain(deletedStep, _workflowTemplate.Steps);
            Assert.Equal(beginningCount - 1, _workflowTemplate.Steps.Count);
            foreach (var step in _workflowTemplate.Steps)
            {
                Assert.Equal(curNumber, step.Number);
                curNumber++;
            }
        }

        [Fact]
        public void RemoveStep_WorkflowTemplate_WithInValidData()
        {
            var number = -1;
            var expectedMassage = "Шаблон не содержит шаг с таким номером";

            var result = _workflowTemplate.RemoveStep(number);

            Assert.False(result.IsSuccess);
            Assert.Equal(expectedMassage, result.Error);
        }
    }
}
