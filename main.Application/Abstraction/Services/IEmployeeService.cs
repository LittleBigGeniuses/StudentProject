using main.Application.Models.Employee;
using Main.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.Application.Abstraction.Services
{
    public interface IEmployeeService
    {
        public Result<Guid> Create(EmployeeCreateDTO createDTO);
        public Result<EmployeeViewDTO> GetById(Guid id);
        public Result<List<EmployeeViewDTO>> GetAllByFilter(EmployeeListFilter filter);
        public Result<bool> DeleteById(Guid id);
        public Result<Guid> UpdateName(Guid id, EmployeeUpdateDTO updateDTO);
        public Result<Guid> UpdateRole(Guid id, EmployeeUpdateDTO updateDTO);
        public Result<Guid> UpdateCompany(Guid id, EmployeeUpdateDTO updateDTO);
    }
}