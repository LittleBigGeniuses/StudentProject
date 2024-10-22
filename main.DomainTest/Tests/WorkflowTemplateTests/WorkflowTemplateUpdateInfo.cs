using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowTemplateDomain;

namespace main.DomainTest.Tests.WorkflowTemplateUpdateIndo
{
    public class WorkflowTemplateUpdate
    {

        private readonly IFixture _fixture;
        private readonly WorkflowTemplate _workflowTemplate;
        public WorkflowTemplateUpdate()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflowTemplate = _fixture.Create<WorkflowTemplate>();
        }


        /// <summary>
        /// Обновление данных с валидными значениями
        /// </summary>
        [Fact]
        public void UpdateInfo_ValidInputs_ShouldUpdateInfoSuccessfully()
        {

            var newName = _fixture.Create<string>().Substring(0, WorkflowTemplate.MinLengthName + 1);
            var newDescription = _fixture.Create<string>();

            var result = _workflowTemplate.UpdateInfo(newName, newDescription);

            Assert.True(result.IsSuccess);
            Assert.Equal(newName.Trim(), _workflowTemplate.Name);
            Assert.Equal(newDescription.Trim(), _workflowTemplate.Description);
            Assert.NotEqual(default, _workflowTemplate.DateUpdate);
        }

        /// <summary>
        /// Невалидные данные обновления (Имя)
        /// </summary>
        [Fact]
        public void UpdateInfo_EmptyName_ShouldReturnFailure()
        {
            var result = _workflowTemplate.UpdateInfo(string.Empty, null);

            Assert.False(result.IsSuccess);
            Assert.Equal("Наименование шаблона не может быть пустым", result.Error);
        }

        /// <summary>
        /// Невалидные данные обновления (Длина имя)
        /// </summary>
        [Fact]
        public void UpdateInfo_ShortName_ShouldReturnFailure()
        {
            var shortName = new string('a', WorkflowTemplate.MinLengthName - 1);

            var result = _workflowTemplate.UpdateInfo(shortName, null);

            Assert.False(result.IsSuccess);
            Assert.Equal($"Длина наименование шаблона не может быть меньше {WorkflowTemplate.MinLengthName}", result.Error);
        }

        /// <summary>
        /// Проверка на неизменяемость даты обновления при вводе тех же данных, что уже содержатся в WorkflowTemplate
        /// </summary>
        [Fact]
        public void UpdateInfo_SameData_DoesNotChangeDateUpdate()
        {
            var originalDateUpdate = _workflowTemplate.DateUpdate;

            var result = _workflowTemplate.UpdateInfo(_workflowTemplate.Name, _workflowTemplate.Description);

            Assert.True(result.IsSuccess);
            Assert.Equal(originalDateUpdate, _workflowTemplate.DateUpdate);
        }
    }
}