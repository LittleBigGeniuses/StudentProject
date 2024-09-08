using main.domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace main.domain.Models.Employee;

/// <summary>
/// Класс сотрудника
/// </summary>
public class Employee : BaseEntity
{
    const int MinLengthName = 5;
    private Employee(string name, long companyId, long roleId)
    {
        Name = name;
        CompanyId = companyId;
        RoleId = roleId;
    }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Индетификатор компании, в которой работает сотрудник
    /// </summary>
    public long CompanyId { get; private set; }

    /// <summary>
    /// Идентификатор долности сотрудника
    /// </summary>
    public long RoleId { get; private set; }

    /// <summary>
    /// Метод создания сотрудника
    /// </summary>
    /// <param name="name">ФИО сотрудника</param>
    /// <param name="companyId">Идентификатор компании</param>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Сущность сотрудника</returns>
    public static Result<Employee> Create(string name, long companyId, long roleId)
    {
        if (String.IsNullOrEmpty(name))
        {
            return Result<Employee>.Failure("ФИО сотрудника не может быть пустым");
        }

        if (name.Trim().Length < MinLengthName)
        {
            return Result<Employee>.Failure($"Длина ФИО сотрудника не может быть меньше {MinLengthName}");
        }

        if (companyId <= 0 )
        {
            return Result<Employee>.Failure("Идентификатор компании некорректен");
        }

        if (roleId <= 0)
        {
            return Result<Employee>.Failure("Идентификатор должности некорректен");
        }

        var employee = new Employee(name, companyId, roleId);

        return Result<Employee>.Success(employee);
    }

    /// <summary>
    /// Обновление ФИО сотрудника
    /// </summary>
    /// <param name="name">ФИО</param>
    /// <returns>Успешность выполнения операции</returns>
    public Result<bool> UpdateName(string name)
    {
        if (String.IsNullOrEmpty(name))
        {
            return Result<bool>.Failure("ФИО не может быть пустым");
        }

        if (name.Trim().Length < MinLengthName)
        {
            return Result<bool>.Failure($"Длина наименование должности не может быть меньше {MinLengthName}");
        }

        Name = name;

        return Result<bool>.Success(true);  
    }

    /// <summary>
    /// Обновление должности сотрудника
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Успешность выполнения операции</returns>
    public Result<bool> UpdateRole(long roleId)
    {
        if (roleId <= 0)
        {
            return Result<bool>.Failure("Некорректное значение идентификатора должности");
        }

        RoleId = roleId;

        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Обновление компании сотрудника
    /// </summary>
    /// <param name="companyId">Идентификатор компании</param>
    /// <returns>Успешность выполнения операции</returns>
    public Result<bool> UpdateCompany(long companyId)
    {
        if (companyId <= 0)
        {
            return Result<bool>.Failure("Некорректное значение идентификатора компании");
        }

        CompanyId = companyId;

        return Result<bool>.Success(true);
    }
}
