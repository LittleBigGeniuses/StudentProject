using Main.Domain.CompanyDomain;
using Main.Domain.CondidateDomain;

namespace Main.DomainTest.Tests.CandidateTests
{
    [TestClass]
    public class CreateTest
    {
        [TestMethod]
        public void TestCreate_ValidInputs_ShouldReturnSuccess()
        {
            // Arrange
            string name = "ValidName";

            // Act
            var result = Candidate.Create(name);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(name, result.Value.Name);
            Assert.AreNotEqual(Guid.Empty, result.Value.Id);
            Assert.IsTrue(result.Value.DateCreate <= DateTime.UtcNow);
            Assert.IsTrue(result.Value.DateUpdate <= DateTime.UtcNow);
        }

        [TestMethod]
        public void TestCreate_EmptyName_ShouldReturnFailure()
        {
            // Arrange
            string name = "";
            // Act
            var result = Candidate.Create(name);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("ФИО соискателя не может быть пустым", result.Error);
        }

        [TestMethod]
        public void TestCreate_NameTooShort_ShouldReturnFailure()
        {
            // Arrange
            string name = "abc";

            // Act
            var result = Candidate.Create(name);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual($"Длина ФИО соискателя не может быть меньше {Candidate.MinLengthName}", result.Error);
        }
    }
}
