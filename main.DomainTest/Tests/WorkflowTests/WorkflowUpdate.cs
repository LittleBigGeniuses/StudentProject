using main.DomainTest.TestTools.Autofixture;
using Main.Domain.WorkflowDomain;

namespace main.DomainTest.Tests.WorkflowTests
{
    public class WorkflowUpdate
    {

        private readonly Fixture _fixture;
        private readonly Workflow _workflow;
        public WorkflowUpdate()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _workflow = _fixture.Create<Workflow>();
        }


        /// <summary>
        /// Обновление данных с валидными значениями
        /// </summary>
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

        /// <summary>
        /// Невалидные данные обновления (Имя)
        /// </summary>
        [Fact]
        public void UpdateInfo_EmptyName_ShouldReturnFailure()
        {
            var result = _workflow.UpdateInfo(string.Empty, null);

            Assert.False(result.IsSuccess);
            Assert.Equal("Наименование рабочего процесса не может быть пустым", result.Error);
        }

        /// <summary>
        /// Невалидные данные обновления (Длина имя)
        /// </summary>
        [Fact]
        public void UpdateInfo_ShortName_ShouldReturnFailure()
        {
            var shortName = new string('a', Workflow.MinLengthName - 1);

            var result = _workflow.UpdateInfo(shortName, null);

            Assert.False(result.IsSuccess);
            Assert.Equal($"Длина наименование рабочего процесса не может быть меньше {Workflow.MinLengthName}", result.Error);
        }

        /// <summary>
        /// Невалидные данные обновления (Описание)
        /// </summary>
        [Fact]
        public void UpdateInfo_ValidDescription_ShouldUpdateDescription()
        {
            var newDescription = _fixture.Create<string>();

            var result = _workflow.UpdateInfo(null, newDescription);

            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription.Trim(), _workflow.Description);
            Assert.NotEqual(default, _workflow.DateUpdate); 
        }

        /// <summary>
        /// Проверка на неизменяемость даты обновления при вводе тех же данных, что уже содержатся в Workflow
        /// </summary>
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
