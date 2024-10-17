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
        }

        [Fact]
        public void Approve_ShouldSucceed_WhenValidEmployeeAndStep()
        { 
            var workflow = _fixture.Create<Workflow>();
            var employee = _fixture.Create<Employee>();

            workflow.SetEmployee(employee);

            var result = workflow.Approve(employee, "Valid feedback");

            Assert.True(result.IsSuccess);
            Assert.Equal(DateTime.UtcNow, workflow.DateUpdate, precision: TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Approve_ShouldFail_WhenEmployeeIsNull()
        {
            var workflow = _fixture.Create<Workflow>();

            var result = workflow.Approve(null, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("employee не может быть пустым", result.Error);
        }


        [Fact]
        public void Approve_ShouldFail_WhenWorkflowStatusIsRejected()
        {

            var workflow = _fixture.Create<Workflow>();
            var employee = _fixture.Create<Employee>();

            var rejectedStep = workflow.Steps.First();
            rejectedStep.Reject(employee,"test"); 

            var result = workflow.Approve(_fixture.Create<Employee>(), "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("Отклоненный рабочий процесс, не может быть одобрен", result.Error);
        }

        [Fact]
        public void Approve_ShouldFail_WhenNoStepInExpectationStatus()
        {
            var workflow = _fixture.Create<Workflow>();
            var employee = _fixture.Create<Employee>();

            foreach (var step in workflow.Steps)
            {
                step.SetEmployee(employee);
                step.Approve(employee, "test");
            }

            var result = workflow.Approve(employee, "Some feedback");

            Assert.True(result.IsFailure);
            Assert.Equal("Рабочий процесс завершен", result.Error);
        }

    }
}
