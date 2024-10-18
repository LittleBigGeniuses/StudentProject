using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.Tests.WorkflowTemplateTests
{
    public class WorkflowTemplateAddStep
    {
        private readonly Fixture _fixture;
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
                "valid description",
                Guid.Empty,
                Guid.NewGuid(),
                $"{Guid.Empty} - некорректный идентификатор сотрудника"            
            };

            yield return new object[]
            {
                "valid description",
                Guid.Empty,
                Guid.NewGuid(),
                $"{Guid.Empty} - некорректный идентификатор должности"
            };
        }

        [Fact]
        public void AddStep_ValidInputs_ShouldAddStepSuccessfully()
        {
            var description = _fixture.Create<string>();
            var employeeId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var result = _workflowTemplate.AddStep(description, employeeId, roleId);

            Assert.True(result.IsSuccess);
            Assert.Equal(description, _workflowTemplate.Steps.Last().Description);
            Assert.Equal(employeeId, _workflowTemplate.Steps.Last().EmployeeId);
            Assert.Equal(roleId, _workflowTemplate.Steps.Last().RoleId);
            Assert.NotEqual(default, _workflowTemplate.DateUpdate);
        }


        [Theory]
        [MemberData(nameof(GetInvalidInputs))]
        public void WorkflowTemplate_AddStep_ShouldReturnFailure(
            string description,
            Guid employeeId, 
            Guid roleId, 
            string expectedErrorMessage)
        {
            var result = _workflowTemplate.AddStep(description, employeeId, roleId);

            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.Error);
        }
    }
}
