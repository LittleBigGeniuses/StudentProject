using main.DomainTest.TestTools.Autofixture;
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
    public class WorkflowApproveStepTests
    {
        private readonly IFixture _fixture;
        private readonly Workflow _workflow;
        private readonly Employee _employee;

        public WorkflowApproveStepTests()
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

        [Fact]
        public void Approve_ShouldSucceed_WhenValidEmployeeAndStep()
        { 
            var result = _workflow.Approve(_employee, "Valid feedback");

            Assert.True(result.IsSuccess);
            Assert.Equal(DateTime.UtcNow, _workflow.DateUpdate, precision: TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Approve_ShouldFail_WhenEmployeeIsNull()
        {
            var result = _workflow.Approve(null, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("employee не может быть пустым", result.Error);
        }


        [Fact]
        public void Approve_ShouldFail_WhenWorkflowStatusIsRejected()
        {
            var rejectedStep = _workflow.Steps.First();

            rejectedStep.Reject(_employee, "test"); 

            var result = _workflow.Approve(_employee, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("Отклоненный рабочий процесс, не может быть одобрен", result.Error);
        }

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
