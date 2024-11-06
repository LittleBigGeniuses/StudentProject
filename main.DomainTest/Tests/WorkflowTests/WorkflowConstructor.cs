using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.DomainTest.Tests.WorkflowTests
{
    public class WorkflowConstructor
    {
        public class WorkflowCtor : Workflow
        {
            public WorkflowCtor(
                Guid id,
                string name,
                string description,
                IReadOnlyCollection<WorkflowStep> steps,
                Guid authorId,
                Guid candidateId,
                Guid templateId,
                Guid companyId,
                DateTime dateCreate,
                DateTime dateUpdate,
                Guid? restartAuthorEmployeeId = null,
                string? restartReason = null,
                DateTime? restartDate = null)
                : base(
                      id, 
                      name, 
                      description, 
                      steps, 
                      authorId, 
                      candidateId, 
                      templateId, 
                      companyId, 
                      dateCreate, 
                      dateUpdate, 
                      restartAuthorEmployeeId, 
                      restartReason, 
                      restartDate)
            { }
        }

        private static readonly IFixture _fixture;
        private static readonly Workflow _workflow;
        static WorkflowConstructor()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflow = _fixture.Create<Workflow>();
        }

        public static IEnumerable<object[]> GetInvalidData()
        {
            var steps = _workflow.Steps;

            yield return new object[]
            {
                Guid.Empty,
                "Valid Name",
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор Процесса"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "",
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "Название процесса не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "Описание процесса не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                null, // Список шагов не определен
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "Список шагов должен быть определен"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                new List<WorkflowStep>(), // Пустой список шагов
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentException),
                "Список шагов не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                new List<WorkflowStep> { null }, // Список шагов содержит null
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentException),
                "Все шаги в списке должны быть определены"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                steps,
                Guid.Empty, // Некорректный идентификатор автора
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор создателя процесса"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.Empty, // Некорректный идентификатор кандидата
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор кандидата"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty, // Некорректный идентификатор шаблона
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор шаблона процесса"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty, // Некорректный идентификатор компании
                DateTime.UtcNow,
                 DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор компании"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.MinValue, // Дата создания - дефолтное значение
                DateTime.UtcNow,
                typeof(ArgumentException),
                "Дата создания не может быть дефолтной."
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.MinValue, // Дата обновления - дефолтное значение
                typeof(ArgumentException),
                "Дата обновления не может быть дефолтной."
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "sh", // Слишком короткое название
                "Valid Description",
                steps,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentException),
                $"Длина наименование процесса не может быть меньше {Workflow.MinLengthName}"
            };
        }

        [Fact]
        public void Workflow_Constructor_With_Valid_Data()
        {
            Guid id = _fixture.Create<Guid>();
            string name = _fixture.Create<string>();
            string description = _fixture.Create<string>();
            var steps = _fixture.Create<List<WorkflowStep>>();
            Guid authorId = _fixture.Create<Guid>();
            Guid candidateId = _fixture.Create<Guid>();
            Guid templateId = _fixture.Create<Guid>();
            Guid companyId = _fixture.Create<Guid>();
            DateTime dateCreate = DateTime.UtcNow;
            DateTime dateUpdate = DateTime.UtcNow;

            var workflow = new WorkflowCtor(
                id,
                name,
                description,
                steps,
                authorId,
                candidateId,
                templateId,
                companyId,
                dateCreate,
                dateUpdate);

            Assert.NotNull(workflow);
            Assert.Equal(id, workflow.Id);
            Assert.Equal(name, workflow.Name);
            Assert.Equal(description, workflow.Description);
            Assert.Equal(steps, workflow.Steps);
            Assert.Equal(authorId, workflow.AuthorId);
            Assert.Equal(candidateId, workflow.CandidateId);
            Assert.Equal(templateId, workflow.TemplateId);
            Assert.Equal(companyId, workflow.CompanyId);
            Assert.Equal(dateCreate, workflow.DateCreate);
            Assert.Equal(dateUpdate, workflow.DateUpdate);
        }

        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void Workflow_Constructor_With_Invalid_Data(
            Guid id,
            string name,
            string description,
            IReadOnlyCollection<WorkflowStep> steps,
            Guid authorId,
            Guid candidateId,
            Guid templateId,
            Guid companyId,
            DateTime dateCreate,
            DateTime dateUpdate,
            Type expectedExceptionType,
            string expectedMessage)
        {
            var exception = Assert.Throws(expectedExceptionType, () =>
                new WorkflowCtor(
                    id,
                    name,
                    description,
                    steps,
                    authorId,
                    candidateId,
                    templateId,
                    companyId,
                    dateCreate,
                    dateUpdate));

            Assert.Contains(expectedMessage, exception.Message);
        }
    }
}
