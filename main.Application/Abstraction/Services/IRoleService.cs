using main.Application.Models.Role;
using Main.Domain.Common;

namespace main.Application.Abstraction.Services
{
    public interface IRoleService
    {
        public Result<Guid> Create(RoleCreateDTO createDTO);
        public Result<RoleViewDTO> GetById(Guid id);
        public Result<List<RoleViewDTO>> GetAllByFilter(RoleListFilter filter);
        public Result<bool> DeleteById(Guid id);
        public Result<Guid> UpdateName(Guid id, RoleUpdateDTO updateDTO);    
    }
}
