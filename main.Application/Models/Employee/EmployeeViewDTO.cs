namespace main.Application.Models.Employee
{
    public class EmployeeViewDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RoleId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
