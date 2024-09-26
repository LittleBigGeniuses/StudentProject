using Main.Domain.Common;

namespace Main.Domain.EmployeeDomain;

/// <summary>
/// Класс сотрудника
/// </summary>
public class Employee
{
    public const int MinLengthName = 5;
    private Employee(
        Guid id, 
        string name, 
        Guid companyId, 
        Guid roleId, 
        DateTime dateCreate, 
        DateTime dateUpdate)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException($"{id} - некорректный идентификатор процесса");
        }

        if (String.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException("ФИО сотрудника не может быть пустым");
        }

        if (companyId == Guid.Empty)
        {
            throw new ArgumentNullException($"{companyId} - некорректный идентификатор компании");
        }

        if (roleId == Guid.Empty)
        {
            throw new ArgumentNullException($"{roleId} - некорректный идентификатор должности");
        }

        Id = id;
        Name = name;
        CompanyId = companyId;
        RoleId = roleId;
        DateCreate = dateCreate;
        DateUpdate = dateUpdate;
    }

    /// <summary>
    /// Метод создания сотрудника
    /// </summary>
    /// <param name="name">ФИО сотрудника</param>
    /// <param name="companyId">Идентификатор компании</param>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Сущность сотрудника</returns>
    public static Result<Employee> Create(string name, Guid companyId, Guid roleId)
    {
        if (name.Trim().Length < MinLengthName)
        {
            return Result<Employee>.Failure($"Длина ФИО сотрудника не может быть меньше {MinLengthName}");
        }

        var employee = new Employee(
            Guid.NewGuid(), 
            name, companyId, 
            roleId, 
            DateTime.UtcNow, 
            DateTime.UtcNow);

        return Result<Employee>.Success(employee);
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime DateCreate { get; }

    /// <summary>
    /// Дата изменения
    /// </summary>
    public DateTime DateUpdate { get; private set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Индетификатор компании, в которой работает сотрудник
    /// </summary>
    public Guid CompanyId { get; private set; }

    /// <summary>
    /// Идентификатор долности сотрудника
    /// </summary>
    public Guid RoleId { get; private set; }


    /// <summary>
    /// Обновление ФИО сотрудника
    /// </summary>
    /// <param name="name">ФИО</param>
    /// <returns>Успешность выполнения операции</returns>
    public Result<bool> UpdateName(string name)
    {

        if (string.IsNullOrEmpty(name))
        {
            return Result<bool>.Failure("ФИО не может быть пустым");
        }

        if (name.Trim().Length < MinLengthName)
        {
            return Result<bool>.Failure($"Длина наименование должности не может быть меньше {MinLengthName}");
        }

        if (name.Trim() != Name)
        {
            Name = name.Trim();
            DateUpdate = DateTime.UtcNow;
        }

        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Обновление должности сотрудника
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Успешность выполнения операции</returns>
    public Result<bool> UpdateRole(Guid roleId)
    {

        if (roleId == Guid.Empty)
        {
            return Result<bool>.Failure("Некорректное значение идентификатора должности");
        }

        if (roleId != RoleId)
        {
            RoleId = roleId;
            DateUpdate = DateTime.UtcNow;
        }

        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Обновление компании сотрудника
    /// </summary>
    /// <param name="companyId">Идентификатор компании</param>
    /// <returns>Успешность выполнения операции</returns>
    public Result<bool> UpdateCompany(Guid companyId)
    {
        if (companyId == Guid.Empty)
        {
            return Result<bool>.Failure("Некорректное значение идентификатора компании");
        }

        if (companyId != CompanyId)
        {
            CompanyId = companyId;
            DateUpdate = DateTime.UtcNow;
        }

        return Result<bool>.Success(true);
    }
}