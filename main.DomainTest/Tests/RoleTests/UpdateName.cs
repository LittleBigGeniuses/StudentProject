using main.DomainTest.TestTools.Autofixture;
using Main.Domain.EmployeeDomain;

namespace main.DomainTest.Tests.RoleTests
{
    public class UpdateName
    {
        private readonly IFixture _fixture;
        private Role _role;

        public UpdateName()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _role = _fixture.Create<Role>();
        }

        public static IEnumerable<object[]> GetInvalidData()
        {
            yield return new object[]
            {
                "",
                "Наименование должности не может быть пустым"
            };

            yield return new object[]
            {
                "te",
                $"Длина наименование должности не может быть меньше {Role.MinLengthName}"
            };
        }

        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void Failed_UpdateName_Role_MethodResult_With_Invalid_Data(
            string name,
            string expectError)
        {
            var dateUpdate = _role.DateUpdate;
            var lastName = _role.Name;
            var resultUpdate = _role.UpdateName(name);

            Assert.True(resultUpdate.IsFailure);
            Assert.Equal(resultUpdate.Error, expectError);
            Assert.Equal(lastName, _role.Name);
            Assert.NotEqual(name, _role.Name);
            Assert.False(dateUpdate < _role.DateUpdate);
        }

        [Fact]
        public void Success_UpdateName_Role_With_Valid_Data()
        {
            var dateUpdate = _role.DateUpdate;
            var lastName = _role.Name;
            var validName = _fixture.Create<string>().Substring(0, Role.MinLengthName + 1);

            var resultUpdate = _role.UpdateName(validName);

            Assert.True(resultUpdate.IsSuccess);
            Assert.NotEqual(_role.Name, lastName);
            Assert.Equal(_role.Name, validName);
            Assert.True(dateUpdate < _role.DateUpdate);
        }

        [Fact]
        public void Success_UpdateName_Role_With_Same_Data()
        {
            var dateUpdate = _role.DateUpdate;
            var lastName = _role.Name;

            var resultUpdate = _role.UpdateName(_role.Name);

            Assert.True(resultUpdate.IsSuccess);
            Assert.Equal(_role.Name, lastName);
            Assert.False(dateUpdate < _role.DateUpdate);
        }
    }
}
