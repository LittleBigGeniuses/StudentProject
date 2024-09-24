using Main.Domain.CompanyDomain;
using Main.Domain.Common;

namespace Main.DomainTest.Tests.CompanyTests
{
    [TestClass]
    public class CompanyCreateTest
    {
        [TestMethod]
        public void TestCreate_ValidInputs_ShouldReturnSuccess()
        {
            // Arrange
            string name = "ValidName";
            string description = "Valid description";

            // Act
            var result = Company.Create(name, description);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(name, result.Value.Name);
            Assert.AreEqual(description, result.Value.Description);
            Assert.AreNotEqual(Guid.Empty, result.Value.Id);
            Assert.IsTrue(result.Value.DateCreate <= DateTime.UtcNow);
            Assert.IsTrue(result.Value.DateUpdate <= DateTime.UtcNow);
        }

        [TestMethod]
        public void TestCreate_EmptyName_ShouldReturnFailure()
        {
            // Arrange
            string name = "";
            string description = "Valid description";

            // Act
            var result = Company.Create(name, description);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Наименование компании не может быть пустым", result.FailureMessage);
        }

        [TestMethod]
        public void TestCreate_NameTooShort_ShouldReturnFailure()
        {
            // Arrange
            string name = "abc";
            string description = "Valid description";

            // Act
            var result = Company.Create(name, description);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual($"Длина наименование шаблона не может быть меньше {Company.MinLengthName}", result.FailureMessage);
        }
    }
}