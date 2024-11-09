using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowTemplateDomain;
namespace main.DomainTest.Tests.WorkflowTemplateTests
{
    public class WorkflowTemplateAddStep
    {
        private readonly IFixture _fixture;
        private readonly WorkflowTemplate _workflowTemplate;

        public WorkflowTemplateAddStep()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflowTemplate = _fixture.Create<WorkflowTemplate>();
        }
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            yield return new object[]
            {
                "valid-description",
                null,
                null,
                "У шага должна быть привязка либо к конкретногому сотруднику либо к должности"
            };

            yield return new object[]
            {
                "valid-description",
                null,
                Guid.Empty,
                $"{Guid.Empty} - некорректное значение для идентификатора должности в шаге"
            };

            yield return new object[]
            {
                "valid-description",
                Guid.Empty,
                null,
                $"{Guid.Empty} - некорректное значение для идентификатора сотрудника в шаге"
            };

            yield return new object[]
            {
                "",
                null,
                Guid.NewGuid(),
                "Описание шаблона процесса не может быть пустым"
            };

            yield return new object[]
            {
                "test-failure",
                null,
                Guid.NewGuid(),
                "Тестовая ошибка при создании шага"
            };
        }


        [Fact]
        public void AddStep_ValidInputs_ShouldAddStepSuccessfully()
        {
            var description = _fixture.Create<string>();
            Guid? employeeId = null;
            Guid? roleId = Guid.NewGuid();
            var primalCount = _workflowTemplate.Steps.Count;

            var resultCreateStep = WorkflowStepTemplate.Create(primalCount + 1, description, employeeId, roleId);
            var result = _workflowTemplate.AddStep(description, employeeId, roleId);

            Assert.True(result.IsSuccess);
            Assert.True(resultCreateStep.IsSuccess);
            Assert.Equal(_workflowTemplate.Steps.Last().Number, primalCount + 1);
            Assert.Equal(description, _workflowTemplate.Steps.Last().Description);
            Assert.Equal(employeeId, _workflowTemplate.Steps.Last().EmployeeId);
            Assert.Equal(roleId, _workflowTemplate.Steps.Last().RoleId);
            Assert.NotEqual(default, _workflowTemplate.DateUpdate);
        }


        [Theory]
        [MemberData(nameof(GetInvalidInputs))]
        public void WorkflowTemplate_AddStep_ShouldReturnFailure(
            string description,
            Guid? employeeId,
            Guid? roleId,
            string expectedErrorMessage)
        {
            var result = _workflowTemplate.AddStep(description, employeeId, roleId);

            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.Error);
        }
    }
}