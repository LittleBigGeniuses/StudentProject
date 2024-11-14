using main.Application.Models.Candidate;
using Main.Domain.Common;

namespace main.Application.Abstraction.Services
{
    public interface ICandidateService
    {
        public Result<Guid> Create(CandidateCreateDTO createDTO);
        public Result<CandidateViewDTO> GetById(Guid id);
        public Result<List<CandidateViewDTO>> GetAllByFilter(CandidateListFilter filter);
        public Result<bool> DeleteById(Guid id);
        public Result<Guid> UpdateName(Guid id, CandidateUpdateDTO updateDTO);
    }
}
