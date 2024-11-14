using main.Application.Models.Company;
using Main.Domain.Common;

namespace main.Application.Abstraction.Services
{
    public interface ICompanyService
    {
        public Result<Guid> Create(CompanyCreateDTO createDTO);
        public Result<CompanyViewDTO> GetById(Guid id);
        public Result<List<CompanyViewDTO>> GetAllByFilter(CompanyListFilter filter);
        public Result<bool> DeleteById(Guid id);
        public Result<Guid> UpdateInfo(Guid id, CompanyUpdateDTO updateDTO);      
    }
}
