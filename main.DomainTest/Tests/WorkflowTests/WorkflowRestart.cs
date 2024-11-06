using main.DomainTest.TestTools.Autofixture;
using Main.Domain.Common;
using Main.Domain.EmployeeDomain;
using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowDomain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.DomainTest.Tests.WorkflowTests
{
    public class WorkflowRestart
    {
        private readonly IFixture _fixture;
        private readonly Workflow _workflow;
        private readonly Employee _employee;
        public WorkflowRestart()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflow = _fixture.Create<Workflow>();
            _employee = _fixture.Create<Employee>();
        }

        /// <summary>
        /// Провал, при null сотруднике
        /// </summary>
        [Fact]
        public void Restart_ShouldReturnFailure_WhenEmployeeIsNull()
        {
            var result = _workflow.Restart(null, "valid reson");

            Assert.True(result.IsFailure);
            Assert.Equal("Сущность сотрудника не может быть пустой", result.Error);
        }

        /// <summary>
        /// Провал, при невалидной причине перезагрузки
        /// </summary>
        [Fact]
        public void Restart_ShouldReturnFailure_WhenRestartReasonIsNullOrEmpty()
        {
            var resultWithEmptyReason = _workflow.Restart(_employee, string.Empty);
            var resultWithNullReason = _workflow.Restart(_employee, null);

            Assert.True(resultWithEmptyReason.IsFailure);
            Assert.Equal("Причина перезапуска должна быть указана", resultWithEmptyReason.Error);

            Assert.True(resultWithNullReason.IsFailure);
            Assert.Equal("Причина перезапуска должна быть указана", resultWithNullReason.Error);
        }

        /// <summary>
        /// Успех, при валидных данных
        /// </summary>
        [Fact]
        public void Restart_ShouldReturnSuccess_WhenInputsAreValid()
        {
            var restartReason = "Valid reason";

            var result = _workflow.Restart(_employee, restartReason);

            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
            Assert.Equal(Status.Expectation, _workflow.Status);
            Assert.Equal(_employee.Id, _workflow.RestartAuthorEmployeeId);
            Assert.Equal(restartReason, _workflow.RestartReason);
            Assert.Equal(DateTime.UtcNow, (DateTime)_workflow.RestartDate!, precision: TimeSpan.FromSeconds(1));
            Assert.Equal(DateTime.UtcNow, _workflow.DateUpdate, precision: TimeSpan.FromSeconds(1));

            foreach (var step in _workflow.Steps)
            {
                Assert.Equal(Status.Expectation, step.Status);
            }
        }
    }
}
