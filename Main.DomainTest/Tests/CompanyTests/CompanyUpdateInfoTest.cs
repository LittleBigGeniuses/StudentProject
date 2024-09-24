using Main.Domain.CompanyDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.DomainTest.Tests.CompanyTests
{
    [TestClass]
    public class CompanyUpdateInfoTest
    {
        [TestMethod]
        public void TestUpdateInfo_ValidInputs_ShouldUpdateNameAndDescription()
        {
            // Arrange
            var company = Company.Create("InitialName", "Initial description").Value;

            // Act
            var result = company.UpdateInfo("NewName", "Updated description");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("NewName", company.Name);
            Assert.AreEqual("Updated description", company.Description);
            Assert.IsTrue(company.DateUpdate <= DateTime.UtcNow);
        }

        [TestMethod]
        public void TestUpdateInfo_ShortName_ShouldReturnFailure()
        {
            // Arrange
            var company = Company.Create("InitialName", "Initial description").Value;

            // Act
            var result = company.UpdateInfo("abc", "Updated description");

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual($"Длина наименование шаблона не может быть меньше {Company.MinLengthName}", result.Error);
        }

        [TestMethod]
        public void TestUpdateInfo_EmptyDescription_ShouldNotUpdateDescription()
        {
            // Arrange
            var company = Company.Create("ValidName", "Initial description").Value;

            // Act
            var result = company.UpdateInfo("NewName", "");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("NewName", company.Name);
            Assert.AreEqual("Initial description", company.Description); // Description should not change
        }

        [TestMethod]
        public void UpdateInfo_WithSameData_DoesNotChangeDateUpdate()
        {
            // Arrange
            var company = Company.Create("InitialName", "Initial description").Value;

            var initialDateUpdate = company.DateUpdate; // Сохраняем текущее значение

            // Act
            var result = company.UpdateInfo(company.Name, company.Description);

            // Assert
            Assert.IsTrue(result.IsSuccess); // Проверяем, что метод успешно завершился
            Assert.AreEqual(initialDateUpdate, company.DateUpdate); // Проверяем, что дата обновления не изменилась
        }
    }
}
