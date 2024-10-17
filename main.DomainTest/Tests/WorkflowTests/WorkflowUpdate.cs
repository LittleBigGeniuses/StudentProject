using AutoFixture;
using main.DomainTest.Customizations;
using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowTemplateDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.DomainTest.Tests.WorkflowTests
{
    public class WorkflowUpdate
    {

        private readonly Fixture _fixture;
        private readonly Workflow _workflow;
        public WorkflowUpdate()
        {
            _fixture = new Fixture();
            _fixture.Customize(new WorkflowTemplateWithStepsCustomization());
            var template = _fixture.Create<WorkflowTemplate>();
            _workflow = Workflow.Create(
                authorId: _fixture.Create<Guid>(),
                candidateId: _fixture.Create<Guid>(),
                template: template).Value!;
        }

        [Fact]
        public void UpdateInfo_ValidInputs_ShouldUpdateInfoSuccessfully()
        {

            var newName = _fixture.Create<string>().Substring(0, Workflow.MinLengthName + 1);
            var newDescription = _fixture.Create<string>();

            var result = _workflow.UpdateInfo(newName, newDescription);

            Assert.True(result.IsSuccess);
            Assert.Equal(newName.Trim(), _workflow.Name);
            Assert.Equal(newDescription.Trim(), _workflow.Description);
            Assert.NotEqual(default, _workflow.DateUpdate);
        }

        [Fact]
        public void UpdateInfo_EmptyName_ShouldReturnFailure()
        {
            var result = _workflow.UpdateInfo(string.Empty, null);

            Assert.False(result.IsSuccess);
            Assert.Equal("Наименование шаблона не может быть пустым", result.Error);
        }

        [Fact]
        public void UpdateInfo_ShortName_ShouldReturnFailure()
        {
            var shortName = new string('a', Workflow.MinLengthName - 1);

            var result = _workflow.UpdateInfo(shortName, null);

            Assert.False(result.IsSuccess);
            Assert.Equal($"Длина наименование шаблона не может быть меньше {Workflow.MinLengthName}", result.Error);
        }

        [Fact]
        public void UpdateInfo_ValidDescription_ShouldUpdateDescription()
        {
            var newDescription = _fixture.Create<string>();

            var result = _workflow.UpdateInfo(null, newDescription);

            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription.Trim(), _workflow.Description);
            Assert.NotEqual(default, _workflow.DateUpdate); 
        }

        [Fact]
        public void UpdateInfo_SameData_DoesNotChangeDateUpdate()
        {
            var originalDateUpdate = _workflow.DateUpdate;

            var result = _workflow.UpdateInfo(_workflow.Name, _workflow.Description);

            Assert.True(result.IsSuccess);
            Assert.Equal(originalDateUpdate, _workflow.DateUpdate);
        }
    }
}
