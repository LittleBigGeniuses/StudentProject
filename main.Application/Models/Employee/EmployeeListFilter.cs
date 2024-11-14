namespace main.Application.Models.Employee
{
    public record EmployeeListFilter(
        string? Name,
        Guid? RoleId,
        Guid CompanyId);
}
