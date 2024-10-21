using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.Tests.WorkflowTests
{
    /// <summary>
    /// Класс тестов создания Workflow
    /// </summary>
    public class WorkflowCreate
    {
        private readonly Fixture _fixture;

        public WorkflowCreate()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();
        }
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            //Не используется фикстура, потому что происходит проверка списка шаблона
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

        /// <summary>
        /// Тестирование создания Workflow при валидных данных
        /// </summary>
        [Fact]
        public void Create_Workflow_With_Valid_Data()
        {

            var validAuthorGuid = _fixture.Create<Guid>();
            var validCandidateGuid = _fixture.Create<Guid>();
            var validTemplate = _fixture.Create<WorkflowTemplate>();

            var workflowCreateResult = Workflow.Create(
                                            validAuthorGuid, 
                                            validCandidateGuid, 
                                            validTemplate);

            Assert.NotNull(workflowCreateResult);
            Assert.True(workflowCreateResult.IsSuccess);
        }


        /// <summary>
        /// Кейс-тест с набором невалидных данных
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="candidateId"></param>
        /// <param name="template"></param>
        /// <param name="expectedErrorMessage"></param>
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
