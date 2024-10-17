using main.DomainTest.Customizations;
using Main.Domain.EmployeeDomain;
using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.Tests.WorkflowTests
{
    public class WorkflowCreate
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var validTemplate = WorkflowTemplate.Create("Valid Name", "Valid Description", Guid.NewGuid()).Value;

            // Неправильный authorId
            yield return new object[]
            {
                Guid.Empty,
                Guid.NewGuid(),
                validTemplate!,
                $"{Guid.Empty} - некорректный идентификатор сотрудника"
            };

            // Неправильный candidateId
            yield return new object[]
            {
                Guid.NewGuid(),
                Guid.Empty,
                validTemplate!,
                $"{Guid.Empty} - некорректный идентификатор кандидата"
            };

            // Неправильный template (null)
            yield return new object[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                null,
                $"template - не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                validTemplate,
                "Предложенный шаблон не содержит шаги"
            };


            // Добавление шага с пустым идентификатором сотрудника
            validTemplate.AddStep("Valid Step", Guid.Empty, null);

            yield return new object[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                validTemplate,
                "Предложенный шаблон не содержит шаги"
            };

            // Ошибка при добавлении шага с некорректным идентификатором роли
            validTemplate.AddStep("Valid Step", null, Guid.Empty);

            yield return new object[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                validTemplate,
                "Предложенный шаблон не содержит шаги"
            };
        }

        [Fact]
        public void Create_Workflow_With_Valid_Data()
        {
            var fixture = new Fixture();
            fixture.Customize(new WorkflowTemplateWithStepsCustomization());

            var validAuthorGuid = fixture.Create<Guid>();
            var validCandidateGuid = fixture.Create<Guid>();
            var validTemplate = fixture.Create<WorkflowTemplate>();

            var workflowCreateResult = Workflow.Create(
                                            validAuthorGuid, 
                                            validCandidateGuid, 
                                            validTemplate);

            Assert.NotNull(workflowCreateResult);
            Assert.True(workflowCreateResult.IsSuccess);
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputs))]
        public void Create_ShouldReturnFailure_WhenInvalidInput(
        Guid authorId,
        Guid candidateId,
        WorkflowTemplate template,
        string expectedErrorMessage)
        {
            var result = Workflow.Create(authorId, candidateId, template);

            Assert.True(result.IsFailure);
            Assert.Equal(expectedErrorMessage, result.Error);
        }
    }
}
