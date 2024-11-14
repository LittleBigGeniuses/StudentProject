using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.Application.Models.Employee
{
    public class EmployeeUpdateDTO
    {
        public string? Name { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
