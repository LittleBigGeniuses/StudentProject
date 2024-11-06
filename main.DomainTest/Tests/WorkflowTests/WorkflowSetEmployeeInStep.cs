using main.DomainTest.TestTools.Autofixture;
using Main.Domain.EmployeeDomain;
using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowDomain.Enum;

namespace main.DomainTest.Tests.WorkflowTests
{
    public class WorkflowSetEmployeeInStep
    {
        private readonly IFixture _fixture;
        private readonly Workflow _workflow;
        private readonly Employee _employee;

        public WorkflowSetEmployeeInStep()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflow = _fixture.Create<Workflow>();
            _employee = _fixture.Create<Employee>();

            foreach (var step in _workflow.Steps)
            {
                step.SetEmployee(_employee);
            }
        }

        /// <summary>
        /// Метод проверки успешного назначения сотрудника, в случае валидных данных
        /// </summary>
        [Fact]
        public void SetEmployee_WithValidData_ShouldSuccess()
        {
            var newEmployee = _fixture.Create<Employee>();
            var step = _workflow.Steps.First();
            var currentEmployeeId = step.EmployeeId;
            var numberStep = step.Number;
            var currentTime = _workflow.DateUpdate;

            var resultOperation = _workflow.SetEmployeeInStep(newEmployee, numberStep);

            Assert.True(resultOperation.IsSuccess);
            Assert.Equal(_workflow.Steps.
                First(s => s.Number == numberStep).EmployeeId, newEmployee.Id);
            Assert.NotEqual(currentTime, _workflow.DateUpdate);
            Assert.NotEqual(step.EmployeeId, currentEmployeeId);
        }

        /// <summary>
        /// Проверка, что при пустной сущности сотрудника будет ошибка
        /// </summary>
        [Fact]
        public void SetEmployee_WithInvalidEmployee_ShouldFailed()
        {
            var step = _workflow.Steps.First();
            var numberStep = step.Number;
            var employeeIdInStep = step.EmployeeId;
            var currentTime = _workflow.DateUpdate;

            var resultOperation = _workflow.SetEmployeeInStep(null, numberStep);

            Assert.False(resultOperation.IsSuccess);
            Assert.Equal("employee не может быть пустым", resultOperation.Error);
            Assert.Equal(currentTime, _workflow.DateUpdate);
            Assert.Equal(employeeIdInStep, step.EmployeeId);
        }

        /// <summary>
        /// Проверка на провал, при завершенном воркфлоу
        /// </summary>
        [Fact]
        public void SetEmployee_IfWorkflow_Completed_ShouldFailed()
        {
            _workflow.Reject(_employee, "valid feedback");

            var step = _workflow.Steps.First();
            var numberStep = step.Number;
            var employeeIdInStep = step.EmployeeId;
            var currentTime = _workflow.DateUpdate;
            var newEmployee = _fixture.Create<Employee>();

            var resultOperation = _workflow.SetEmployeeInStep(newEmployee, numberStep);

            Assert.False(resultOperation.IsSuccess);
            Assert.Equal("Рабочий процесс завершен", resultOperation.Error);
            Assert.NotEqual(newEmployee.Id, step.EmployeeId);
            Assert.Equal(employeeIdInStep, step.EmployeeId);
            Assert.Equal(currentTime, _workflow.DateUpdate);
        }

        /// <summary>
        /// Тест, при некорректном вводе номера шага, ожидается провал
        /// </summary>
        [Fact]
        public void SetEmployee_WithInvaled_NumberStep_ShouldFailed()
        {
            var newEmployee = _fixture.Create<Employee>();
            var numberStep = 0;
            var currentTime = _workflow.DateUpdate;

            var resultOperation = _workflow.SetEmployeeInStep(newEmployee, numberStep);

            Assert.False(resultOperation.IsSuccess);
            Assert.Equal($"Шаг с номером {numberStep} не найден", resultOperation.Error);
            Assert.Equal(currentTime, _workflow.DateUpdate);
        }

        /// <summary>
        /// Тест, на провал, при попытке назначить на завершенный шаг
        /// </summary>
        [Fact]
        public void SetEmployee_IfWorkflowStep_Completed_ShouldFailed()
        {
            _workflow.Approve(_employee, "valid feedback");

            var step = _workflow.Steps.First(s => s.Status == Status.Approved);
            var numberStep = step.Number;
            var currentEmployeeId = step.EmployeeId;
            var currentTime = _workflow.DateUpdate;
            var newEmployee = _fixture.Create<Employee>();

            var resultOperation = _workflow.SetEmployeeInStep(newEmployee, numberStep);

            Assert.False(resultOperation.IsSuccess);
            Assert.Equal($"Шаг {numberStep} завершен", resultOperation.Error);
            Assert.NotEqual(newEmployee.Id, step.EmployeeId);
            Assert.Equal(currentEmployeeId, step.EmployeeId);
            Assert.Equal(currentTime, _workflow.DateUpdate);
        }

        /// <summary>
        /// Провал, при провали назначения сотрудника в методе SetEmployee в шаге рабочего процесса
        /// </summary>
        [Fact]
        public void SetEmployee_IfWorkflowStep_Return_Failed_ShouldFailed()
        {
            var newEmployee = _fixture.Create<Employee>();
            newEmployee.UpdateName("test-name-for-failed-result");

            var step = _workflow.Steps.First();
            var currentEmployeeId = step.EmployeeId;
            var numberStep = step.Number;
            var currentTime = _workflow.DateUpdate;

            var resultOperation = _workflow.SetEmployeeInStep(newEmployee, numberStep);

            Assert.False(resultOperation.IsSuccess);
            Assert.Equal("Отработал тестовый провал назначения сотрудника", resultOperation.Error);
            Assert.NotEqual(newEmployee.Id, step.EmployeeId);
            Assert.Equal(currentEmployeeId, step.EmployeeId);
            Assert.Equal(currentTime, _workflow.DateUpdate);
        }

        /// <summary>
        /// Проверка неизменяемости даты обновления, при вводе уже имеющихся данных
        /// </summary>
        [Fact]
        public void SetEmployee_Same_Employee_ShouldSuccess_And_Immutability_DateUpdate()
        {
            var step = _workflow.Steps.First();
            var currentEmployeeId = step.EmployeeId;
            var numberStep = step.Number;
            var currentTime = _workflow.DateUpdate;

            var resultOperation = _workflow.SetEmployeeInStep(_employee, numberStep);

            Assert.True(resultOperation.IsSuccess);
            Assert.Equal(currentTime, _workflow.DateUpdate);
        }

    }
}
