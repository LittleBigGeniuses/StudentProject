using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowTemplateDomain;

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
        public void RemoveStep_ValidInputs_ShouldRemoveStepSuccessfully()
        {
            int currentStep = 1;
            var stepsCount = _workflowTemplate.Steps.Count;
            var stepNumber = _fixture.Create<int>() % stepsCount + 1;
            var step = _workflowTemplate.Steps.First(x => x.Number == stepNumber);

            var result = _workflowTemplate.RemoveStep(stepNumber);

            Assert.True(result.IsSuccess);
            Assert.Equal(_workflowTemplate.Steps.Count, stepsCount - 1);
            Assert.DoesNotContain(step, _workflowTemplate.Steps);
            foreach (var testStep in _workflowTemplate.Steps)
            {
                Assert.Equal(currentStep, testStep.Number);
                currentStep++;
            }
        }

        [Fact]
        public void RemoveStep_InvalidInput_ShouldReturenFailure()
        {
            var stepNumber = _workflowTemplate.Steps.Count + 1;
            var expectedErrorMessage = $"Шаблон не содержит с таким номером {stepNumber}";

            var result = _workflowTemplate.RemoveStep(stepNumber);

            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.Error);
        }
    }
}
