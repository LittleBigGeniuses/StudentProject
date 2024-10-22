using Main.Domain.EmployeeDomain;

namespace main.DomainTest.Tests.RoleTests
{
    public class RoleCreate
    {
        private readonly IFixture _fixture;

        public RoleCreate()
        {
            _fixture = new Fixture();
        }

        public static IEnumerable<object[]> GetInvalidData()
        {
            yield return new object[]
            {
                "",
                Guid.NewGuid(),
                "Наименование должности не может быть пустым"
            };

            yield return new object[]
            {
                "Valid Name",
                Guid.Empty,
                $"{Guid.Empty} - некорректный идентификатор компании"
            };

            yield return new object[]
            {
                "te",
                Guid.NewGuid(),
                $"Длина наименования должности не может быть меньше {Role.MinLengthName}"
            };
        }

        [Fact]
        public void Success_Role_Create_StaticMethod_With_Valid_Data()
        {
            var name = _fixture.Create<string>().Substring(0, Role.MinLengthName + 1);
            var companyId = _fixture.Create<Guid>();

            var resultCreateRole = Role.Create(name, companyId);
            var role = resultCreateRole.Value;

            Assert.True(resultCreateRole.IsSuccess);
            Assert.NotNull(role);
            Assert.Equal(name, role.Name);
            Assert.Equal(companyId, role.CompanyId);
        }

        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void Failed_Role_Create_With_Invalid_Data(
            string name,
            Guid companyId,
            string expectedError)
        {
            var resultCreateRole = Role.Create(name, companyId);
            var role = resultCreateRole.Value;

            Assert.True(resultCreateRole.IsFailure);
            Assert.False(resultCreateRole.IsSuccess);
            Assert.Null(role);
            Assert.Equal(resultCreateRole.Error, expectedError);
        }
    }
}
