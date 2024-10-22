using main.DomainTest.TestTools.Autofixture;
using Main.Domain.EmployeeDomain;
using Main.Domain.WorkflowDomain;

namespace main.DomainTest.Tests.WorkflowTests
{
    /// <summary>
    /// Класс тестирования одобрения шага workflow
    /// </summary>
    public class WorkflowApproveStep
    {
        private readonly IFixture _fixture;
        private readonly Workflow _workflow;
        private readonly Employee _employee;

        public WorkflowApproveStep()
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
        /// Проверка успешного одобрения шага
        /// </summary>
        [Fact]
        public void Approve_ShouldSucceed_WhenValidEmployeeAndStep()
        { 
            var result = _workflow.Approve(_employee, "Valid feedback");

            Assert.True(result.IsSuccess);
            Assert.Equal(DateTime.UtcNow, _workflow.DateUpdate, precision: TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Проверка ошибки при пустом сотруднике
        /// </summary>
        [Fact]
        public void Approve_ShouldFail_WhenEmployeeIsNull()
        {
            var result = _workflow.Approve(null, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("employee не может быть пустым", result.Error);
        }


        /// <summary>
        /// Проверка ошибки при статусе отказа
        /// </summary>
        [Fact]
        public void Approve_ShouldFail_WhenWorkflowStatusIsRejected()
        {
            var rejectedStep = _workflow.Steps.First();

            rejectedStep.Reject(_employee, "test"); 

            var result = _workflow.Approve(_employee, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("Отклоненный рабочий процесс, не может быть одобрен", result.Error);
        }

        /// <summary>
        /// Проверка одобрения при завершенности workflow
        /// </summary>
        [Fact]
        public void Approve_ShouldFail_WhenNoStepInExpectationStatus()
        {
            foreach (var step in _workflow.Steps)
            {
                step.Approve(_employee, "test");
            }

            var result = _workflow.Approve(_employee, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("Рабочий процесс завершен", result.Error);
        }

    }
}
