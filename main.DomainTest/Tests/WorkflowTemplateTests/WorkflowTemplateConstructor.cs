using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowTemplateDomain;


namespace main.DomainTest.Tests.WorkflowTemplateTests
{
    public class WorkflowTemplateConstructor
    {
        public class WorkflowTemplateCtor : WorkflowTemplate
        {
            public WorkflowTemplateCtor(
                Guid id,
                string name,
                string description,
                List<WorkflowStepTemplate> stepTemplates,
                Guid companyId,
                DateTime dateCreate,
                DateTime dateUpdate)
                : base(
                      id,
                      name,
                      description,
                      stepTemplates,
                      companyId,
                      dateCreate,
                      dateUpdate)
            { }
        }

        private static readonly IFixture _fixture;
        static WorkflowTemplateConstructor()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();
        }

        public static IEnumerable<object[]> GetInvalidData()
        {

            yield return new object[]
            {
                Guid.Empty,
                "Valid Name",
                "Valid Description",
                new List<WorkflowStepTemplate>(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                $"{Guid.Empty} - некорректный идентификатор шаблона"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "",
                "Valid Description",
                new List<WorkflowStepTemplate>(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "Наименование шаблона не может быть пустым"
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "",
                new List<WorkflowStepTemplate>(),
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
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentNullException),
                "Список шагов шаблона должен быть определен"
            };

           


            yield return new object[]
            {
                Guid.NewGuid(),
                "Valid Name",
                "Valid Description",
                new List<WorkflowStepTemplate>(),
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
                new List<WorkflowStepTemplate>(),
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
                new List<WorkflowStepTemplate>(),
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
                new List<WorkflowStepTemplate>(),
                Guid.NewGuid(),              
                DateTime.UtcNow,
                DateTime.UtcNow,
                typeof(ArgumentException),
                $"Длина наименование шаблона не может быть меньше {WorkflowTemplate.MinLengthName}"
            };
        }

        [Fact]
        public void WorkflowTemplate_Constructor_With_Valid_Data()
        {
            Guid id = _fixture.Create<Guid>();
            string name = _fixture.Create<string>();
            string description = _fixture.Create<string>();
            var steps = new List<WorkflowStepTemplate>();
            Guid companyId = _fixture.Create<Guid>();
            DateTime dateCreate = DateTime.UtcNow;
            DateTime dateUpdate = DateTime.UtcNow;

            var workflowTemplate = new WorkflowTemplateCtor(
                id,
                name,
                description,
                steps,
                companyId,
                dateCreate,
                dateUpdate);

            Assert.NotNull(workflowTemplate);
            Assert.Equal(id, workflowTemplate.Id);
            Assert.Equal(name, workflowTemplate.Name);
            Assert.Equal(description, workflowTemplate.Description);
            Assert.Equal(steps, workflowTemplate.Steps);
            Assert.Equal(companyId, workflowTemplate.CompanyId);
            Assert.Equal(dateCreate, workflowTemplate.DateCreate);
            Assert.Equal(dateUpdate, workflowTemplate.DateUpdate);
        }

        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void Workflow_Constructor_With_Invalid_Data(
            Guid id,
            string name,
            string description,
            List<WorkflowStepTemplate> steps,            
            Guid companyId,
            DateTime dateCreate,
            DateTime dateUpdate,
            Type expectedExceptionType,
            string expectedMessage)
        {
            var exception = Assert.Throws(expectedExceptionType, () =>
                new WorkflowTemplateCtor(
                    id,
                    name,
                    description,
                    steps,
                    companyId,
                    dateCreate,
                    dateUpdate));

            Assert.Contains(expectedMessage, exception.Message);
        }
    }
}