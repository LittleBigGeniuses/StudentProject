using Main.Domain.EmployeeDomain;
using Main.Domain.WorkflowDomain;
using main.DomainTest.TestTools.Autofixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.DomainTest.Tests.WorkflowTests
{
    public class WorkflowRejectStep
    {
        private readonly IFixture _fixture;
        private readonly Workflow _workflow;
        private readonly Employee _employee;

        public WorkflowRejectStep()
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
        public void Reject_ShouldSucceed_WhenValidEmployeeAndStep()
        {
            var result = _workflow.Reject(_employee, "Valid feedback");

            Assert.True(result.IsSuccess);
            Assert.Equal(DateTime.UtcNow, _workflow.DateUpdate, precision: TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Проверка ошибки при пустом сотруднике
        /// </summary>
        [Fact]
        public void Reject_ShouldFail_WhenEmployeeIsNull()
        {
            var result = _workflow.Reject(null, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("employee не может быть пустым", result.Error);
        }

        /// <summary>
        /// Проверка одобрения при завершенности workflow
        /// </summary>
        [Fact]
        public void Reject_ShouldFail_WhenNoStepInExpectationStatus()
        {
            foreach (var step in _workflow.Steps)
            {
                step.Approve(_employee, "test");
            }

            var result = _workflow.Reject(_employee, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("Рабочий процесс завершен", result.Error);
        }

        /// <summary>
        /// Проверка вывода ошибки, если метод отказа внутри шага вернул провал
        /// </summary>
        [Fact]
        public void Reject_ShouldFailed_WhenFailedApproveStep()
        {
            var result = _workflow.Reject(_employee, "test-failed-reject");

            Assert.True(result.IsFailure);
            Assert.Equal("Отработал тестовый провал отказа шага", result.Error);
        }
    }
}
