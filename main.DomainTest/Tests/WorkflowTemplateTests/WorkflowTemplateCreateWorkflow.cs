using Main.Domain.WorkflowTemplateDomain;
using main.DomainTest.TestTools.Autofixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.DomainTest.Tests.WorkflowTemplateTests
{
    public class WorkflowTemplateCreateWorkflow
    {
        private readonly IFixture _fixture;
        private readonly WorkflowTemplate _workflowTemplate;

        public WorkflowTemplateCreateWorkflow()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflowTemplate = _fixture.Create<WorkflowTemplate>();
        }
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            yield return new object[]
            {
                false,
                null,
                Guid.NewGuid(),
                $"{Guid.Empty} - некорректный идентификатор сотрудника"
            };

            yield return new object[]
            {
                false,
                Guid.NewGuid(),
                null,
                $"{Guid.Empty} - некорректный идентификатор кандидата"
            };

            yield return new object[]
            {
                true,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Тестовая ошибка при создании процесса"
            };
        }


        [Fact]
        public void CreateWorkflow_ValidInputs_ShouldCreateWorkflowSuccessfully()
        {
            var description = _fixture.Create<string>();
            Guid authorId = Guid.NewGuid();
            Guid candidateId = Guid.NewGuid();
            var primalCount = _workflowTemplate.Steps.Count;
 
            var result = _workflowTemplate.CreateWorkflow(authorId, candidateId);

            Assert.True(result.IsSuccess);
            Assert.Equal(authorId, result.Value.AuthorId);
            Assert.Equal(candidateId, result.Value.CandidateId);
            Assert.Equal(result.Value.Name, _workflowTemplate.Name);
        }


        [Theory]
        [MemberData(nameof(GetInvalidInputs))]
        public void WorkflowTemplate_CreateWorkflow_ShouldReturnFailure(
            bool needInvalidInputFromTemplate,
            Guid authorId,
            Guid candidateId,
            string expectedErrorMessage)
        {
            if (needInvalidInputFromTemplate)
            {
                _workflowTemplate.UpdateInfo("test-failure", "test-failure");
            }

            var result = _workflowTemplate.CreateWorkflow(authorId, candidateId);

            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.Error);
        }
    }
}

