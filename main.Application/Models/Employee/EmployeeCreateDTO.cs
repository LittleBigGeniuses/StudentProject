namespace main.Application.Models.Employee
{
    public class EmployeeCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Guid RoleId { get; set; }
    }
}
