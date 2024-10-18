using main.DomainTest.TestTools.Autofixture;
using Main.Domain.CompanyDomain;
using System.Runtime.CompilerServices;

namespace main.DomainTest.Tests.CompanyTests
{
    public class CompanyUpdateInfo
    {
        private readonly IFixture _fixture;
        private readonly Company _company;

        public CompanyUpdateInfo()
        {
            _fixture = new Fixture();
            _fixture.FixtureCustomization();

            _company = _fixture.Create<Company>();
        }


        [Fact]
        public void Update_Only_Name_With_Valid_Data()
        {
            var newValidName = _fixture.Create<string>();
            var initialDateUpdate = _company.DateUpdate;

            var resultUpdate = _company.UpdateInfo(newValidName, null);           

            Assert.True(resultUpdate.IsSuccess);
            Assert.Equal(_company.Name, newValidName);
            Assert.True(_company.DateUpdate > initialDateUpdate);
        }

        [Fact]
        public void Update_Only_Description_With_Valid_Data()
        {
            var newValidDescription = _fixture.Create<string>();
            var initialDateUpdate = _company.DateUpdate;

            var resultUpdate = _company.UpdateInfo(null, newValidDescription);

            Assert.True(resultUpdate.IsSuccess);
            Assert.Equal(_company.Description, newValidDescription);
            Assert.True(_company.DateUpdate > initialDateUpdate);
        }

        [Fact]
        public void Update_Name_And_Description_With_Valid_Data()
        {
            var newValidName = _fixture.Create<string>();
            var newValidDescription = _fixture.Create<string>();
            var initialDateUpdate = _company.DateUpdate;

            var resultUpdate = _company.UpdateInfo(newValidName, newValidDescription);

            Assert.True(resultUpdate.IsSuccess);
            Assert.Equal(_company.Name, newValidName);
            Assert.Equal(_company.Description, newValidDescription);
            Assert.True(_company.DateUpdate > initialDateUpdate);
        }

        [Fact]
        public void Update_Only_Name_With_Short_Data()
        {
            var newShortName = _fixture.Create<string>().Substring(0, Company.MinLengthName - 1);
            var initialDateUpdate = _company.DateUpdate;

            var resultUpdate = _company.UpdateInfo(newShortName, null);

            Assert.True(resultUpdate.IsFailure);
            Assert.NotEqual(_company.Name, newShortName);
            Assert.True(_company.DateUpdate == initialDateUpdate);
        }

        [Fact]
        public void Check_DateUpdate_With_Same_Data()
        {
            var name = _company.Name;
            var description = _company.Description;
            var initialDateUpdate = _company.DateUpdate;

            var resultUpdate = _company.UpdateInfo(name, description);

            Assert.True(resultUpdate.IsSuccess);
            Assert.True(_company.DateUpdate == initialDateUpdate);
        }
    }
}
