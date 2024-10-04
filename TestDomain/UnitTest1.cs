using System;
using NUnit.Framework;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestFixture]
public class CompanyTestsNUnit
{
    private Fixture _fixture;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    public void CreateCompany_WithValidData_ShouldReturnSuccess()
    {
        string name = _fixture.Create<string>().Substring(0, 10);
        string description = _fixture.Create<string>().Substring(0, 50);

        var result = Company.Create(name, description);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(name.Trim(), result.Value.Name);
        Assert.AreEqual(description.Trim(), result.Value.Description);
    }

    [Test]
    public void CreateCompany_WithInvalidShortName_ShouldReturnFailure()
    {
        string shortName = "AB";
        string description = _fixture.Create<string>().Substring(0, 50);

        var result = Company.Create(shortName, description);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual($"Длина наименования компании не может быть меньше {Company.MinLengthName}", result.Error);
    }

    [Test]
    public void CreateCompany_WithEmptyName_ShouldReturnFailure()
    {
        string description = _fixture.Create<string>().Substring(0, 50);

        var result = Company.Create("", description);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Наименование компании не может быть пустым", result.Error);
    }

    [Test]
    public void CreateCompany_WithEmptyDescription_ShouldReturnFailure()
    {
        string name = _fixture.Create<string>().Substring(0, 10);

        var result = Company.Create(name, "");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Описание компании не может быть пустым", result.Error);
    }

    [Test]
    public void UpdateCompanyInfo_WithValidData_ShouldUpdateInfo()
    {
        string name = _fixture.Create<string>().Substring(0, 10);
        string description = _fixture.Create<string>().Substring(0, 50);

        var result = Company.Create(name, description);
        var company = result.Value;

        string newName = _fixture.Create<string>().Substring(0, 10);
        string newDescription = _fixture.Create<string>().Substring(0, 50);

        var updateResult = company.UpdateInfo(newName, newDescription);

        Assert.IsTrue(updateResult.IsSuccess);
        Assert.AreEqual(newName, company.Name);
        Assert.AreEqual(newDescription, company.Description);
        Assert.AreNotEqual(company.DateCreate, company.DateUpdate);
    }

    [Test]
    public void UpdateCompanyInfo_WithSameData_ShouldNotUpdateInfo()
    {
        string name = _fixture.Create<string>().Substring(0, 10);
        string description = _fixture.Create<string>().Substring(0, 50);

        var result = Company.Create(name, description);
        var company = result.Value;

        var updateResult = company.UpdateInfo(name, description); // Тоже самое имя и описание

        Assert.IsTrue(updateResult.IsSuccess);
        Assert.AreEqual(name, company.Name);
        Assert.AreEqual(description, company.Description);
        Assert.AreEqual(company.DateCreate, company.DateUpdate); // Дата обновления не должна измениться
    }
}
