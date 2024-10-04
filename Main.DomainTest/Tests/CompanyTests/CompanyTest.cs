using System;
using Xunit;
using AutoFixture;
using Main.Domain.CompanyDomain;
using Assert = Xunit.Assert;

public class CompanyTestsWithInlineData
{
    private readonly Fixture _fixture;

    public CompanyTestsWithInlineData()
    {
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData("ValidCompanyName", "ValidDescription", true)]
    [InlineData("Short", "ValidDescription", true)]
    [InlineData("", "ValidDescription", false)]
    [InlineData("ValidCompanyName", "", false)]
    public void CreateCompany_WithDifferentData_ShouldReturnExpectedResult(string name, string description, bool expectedResult)
    {
        var result = Company.Create(name, description);

        Assert.Equal(expectedResult, result.IsSuccess);
    }

    [Theory]
    [InlineData("", "ValidDescription", "Наименование компании не может быть пустым")]
    [InlineData("ValidCompanyName", "", "Описание компании не может быть пустым")]
    public void CreateCompany_WithInvalidData_ShouldReturnExpectedError(string name, string description, string expectedError)
    {
        var result = Company.Create(name, description);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedError, result.Error);
    }

    [Theory]
    [InlineData("NewCompanyName", "NewDescription", true)]
    [InlineData(null, null, true)]
    public void UpdateCompanyInfo_ShouldBehaveAsExpected(string newName, string newDescription, bool isChangeExpected)
    {
        string name = _fixture.Create<string>().Substring(0, 10);
        string generatedString = _fixture.Create<string>();
        string description = generatedString.Length >= 30 ? generatedString.Substring(0, 30) : generatedString;

        var result = Company.Create(name, description);
        var company = result.Value;

        var updateResult = company.UpdateInfo(newName, newDescription);

        Assert.Equal(isChangeExpected, updateResult.IsSuccess);
        if (isChangeExpected)
        {
            Assert.Equal(newName ?? name, company.Name);
            Assert.Equal(newDescription ?? description, company.Description);
        }
    }
}
