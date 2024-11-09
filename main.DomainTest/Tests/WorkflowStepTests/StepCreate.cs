using main.DomainTest.TestTools.Autofixture;
using Main.Domain.CandidateDomain;
using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowDomain.Enum;
using Main.Domain.WorkflowTemplateDomain;
using System.Reflection;
using System.Runtime.Serialization;

namespace main.DomainTest.Tests.WorkflowStepTests
{
    public class StepCreate
    {
        private readonly IFixture _fixture;
        public StepCreate() 
        { 
            _fixture = new Fixture();
            _fixture.FixtureCustomization();
        }

        /// <summary>
        /// Успешное создание шага при валидных данных
        /// </summary>
        [Fact]
        public void Create_With_ValidData_Should_Success()
        {
            var validCandidateId = _fixture.Create<Guid>();
            var validStepTemplate = _fixture.Create<WorkflowStepTemplate>();

            var numberStepTemplate = validStepTemplate.Number;
            var descriptionStepTemplate = validStepTemplate.Description;
            var employeeIdStepTemplate = validStepTemplate.EmployeeId;
            var roleIdStepTemplate = validStepTemplate.RoleId;

            var resultCreate = WorkflowStep.Create(validCandidateId, validStepTemplate);
            var step = resultCreate.Value;

            Assert.True(resultCreate.IsSuccess);
            Assert.NotNull(step);
            Assert.Equal(numberStepTemplate, step.Number);
            Assert.Equal(descriptionStepTemplate, step.Description);
            Assert.Equal(employeeIdStepTemplate, step.EmployeeId);
            Assert.Equal(roleIdStepTemplate, step.RoleId);
            Assert.Equal(Status.Expectation, step.Status);
        }

        /// <summary>
        /// Провал создания при null роли и сотруднике в шаблоне шага
        /// </summary>
        [Fact]
        public void Create_ShouldFail_WhenBothEmployeeIdAndRoleIdAreNull()
        {
            // Создаем экземпляр WorkflowStepTemplate без вызова конструктора
            var stepTemplate = (WorkflowStepTemplate)FormatterServices.GetUninitializedObject(typeof(WorkflowStepTemplate));

            // Устанавливаем значения для необходимых полей напрямую через рефлексию
            typeof(WorkflowStepTemplate).GetField("<EmployeeId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(stepTemplate, null);
            typeof(WorkflowStepTemplate).GetField("<RoleId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(stepTemplate, null);

            // Устанавливаем значения для остальных полей, если они доступны только для чтения
            typeof(WorkflowStepTemplate).GetField("<Number>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(stepTemplate, 1);
            typeof(WorkflowStepTemplate).GetField("<Description>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(stepTemplate, "Test Description");
            typeof(WorkflowStepTemplate).GetField("<DateCreate>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(stepTemplate, DateTime.UtcNow);
            typeof(WorkflowStepTemplate).GetField("<DateUpdate>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(stepTemplate, DateTime.UtcNow);

            // Проверка логики создания шага
            var result = WorkflowStep.Create(Guid.NewGuid(), stepTemplate);

            // Проверяем, что результат неудачный из-за отсутствия EmployeeId и RoleId
            Assert.True(result.IsFailure);
            Assert.Equal("У шага должна быть привязка к конкретному сотруднику или должности", result.Error);
        }

        /// <summary>
        /// Провал, при пустом id кандидата
        /// </summary>
        [Fact]
        public void Create_ShouldFail_WhenEmptyCandidateId()
        {
            var validStepTemplate = _fixture.Create<WorkflowStepTemplate>();
            var emptyCandidateId = Guid.Empty;

            var resultCreate = WorkflowStep.Create(emptyCandidateId, validStepTemplate);
            var step = resultCreate.Value;

            Assert.True(resultCreate.IsFailure);
            Assert.Null(step);
            Assert.Equal($"{emptyCandidateId} - некорректный идентификатор кандидата", resultCreate.Error);
        }

        [Fact]
        public void Create_ShouldFail_WhenStepTemplateIsNull()
        {
            var validCandidateId = _fixture.Create<Guid>();
            WorkflowStepTemplate nullStepTemplate = null;

            var resultCreate = WorkflowStep.Create(validCandidateId, nullStepTemplate);
            var step = resultCreate.Value;

            Assert.True(resultCreate.IsFailure);
            Assert.Null(step);
            Assert.Equal("Шаблон шага не может быть неопределен", resultCreate.Error);
        }
    }
}
