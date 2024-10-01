using Main.Domain.EmployeeDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.DomainTest.Tests.EmployeeTests
{
    [TestClass]
    internal class CreateTest
    {
        [TestMethod]
        public void TestCreate_ValidInputs_ShouldReturnSuccess()
        {
            // Arrange
            string name = "Valid Name";
            Guid companyId = Guid.NewGuid();
            Guid roleId = Guid.NewGuid();

            // Act
            var result = Employee.Create(name, companyId, roleId);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(name, result.Value.Name);
            Assert.AreEqual(companyId, result.Value.CompanyId);
            Assert.AreEqual(roleId, result.Value.RoleId);
            Assert.AreNotEqual(Guid.Empty, result.Value.Id);
            Assert.IsTrue(result.Value.DateCreate <= DateTime.UtcNow);
            Assert.IsTrue(result.Value.DateUpdate <= DateTime.UtcNow);
        }

        [TestMethod]
        public void TestCreate_EmptyName_ShouldRetuenFailure()
        {
            //Arrange
            string name = "";
            Guid companyId = Guid.NewGuid();
            Guid roleId = Guid.NewGuid();

            //Act
            var result = Employee.Create(name, companyId, roleId);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, "ФИО сотрудника не может быть пустым");
        }

        [TestMethod]
        public void TestCreate_NameTooShort_ShouldRetuenFailure()
        {
            //Arrange
            string name = "abc";
            Guid companyId = Guid.NewGuid();
            Guid roleId = Guid.NewGuid();

            //Act
            var result = Employee.Create(name, companyId, roleId);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, $"Длина ФИО сотрудника не может быть меньше {Employee.MinLengthName}");
        }

        [TestMethod]
        public void TestCreate_CompanyIdIsIncorrect_ShouldRetuenFailure()
        {
            //Arrange
            string name = "Valid Name";
            Guid companyId = Guid.Empty;
            Guid roleId = Guid.NewGuid();

            //Act
            var result = Employee.Create(name, companyId, roleId);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, "Идентификатор компании некорректен");
        }

        [TestMethod]
        public void TestCreate_RoleIdIsIncorrect_ShouldRetuenFailure()
        {
            //Arrange
            string name = "Valid Name";
            Guid companyId = Guid.NewGuid();
            Guid roleId = Guid.Empty;

            //Act
            var result = Employee.Create(name, companyId, roleId);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, "Идентификатор должности некорректен");
        }
    }
}
