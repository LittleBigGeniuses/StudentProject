using main.domain.Common;
using System;

namespace main.domain.Models.Employee;

/// <summary>
/// Класс сотрудника
/// </summary>
public class Employee : BaseEntity
{
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
}
