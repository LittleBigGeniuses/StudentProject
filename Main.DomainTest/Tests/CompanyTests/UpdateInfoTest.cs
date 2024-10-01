using Main.Domain.CompanyDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.DomainTest.Tests.CompanyTests
{
    [TestClass]
    public class UpdateInfoTest
    {
        private Company _company;

        [TestInitialize]
        public void Initialize()
        {
            // Инициализация объекта перед каждым тестом
            _company = Company.Create("InitialName", "Initial description").Value;
        }

        [TestMethod]
        public void TestUpdateInfo_ValidInputs_ShouldUpdateNameAndDescription()
        {
            // Act
            var result = _company.UpdateInfo("NewName", "Updated description");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("NewName", _company.Name);
            Assert.AreEqual("Updated description", _company.Description);
            Assert.IsTrue(_company.DateUpdate <= DateTime.UtcNow);
        }

        [TestMethod]
        public void TestUpdateInfo_ShortName_ShouldReturnFailure()
        {
            // Act
            var result = _company.UpdateInfo("abc", "Updated description");

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual($"Длина наименование шаблона не может быть меньше {Company.MinLengthName}", result.Error);
        }

        [TestMethod]
        public void TestUpdateInfo_EmptyDescription_ShouldNotUpdateDescription()
        {
            // Act
            var result = _company.UpdateInfo("NewName", "");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("NewName", _company.Name);
            Assert.AreEqual("Initial description", _company.Description); 
        }

        [TestMethod]
        public void UpdateInfo_WithSameData_DoesNotChangeDateUpdate()
        {
            var initialDateUpdate = _company.DateUpdate; 

            // Act
            var result = _company.UpdateInfo(_company.Name, _company.Description);

            // Assert
            Assert.IsTrue(result.IsSuccess); 
            Assert.AreEqual(initialDateUpdate, _company.DateUpdate); 
        }
    }
}
