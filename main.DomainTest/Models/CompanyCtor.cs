using Main.Domain.CompanyDomain;

namespace main.DomainTest.Models
{
    public class CompanyCtor : Company
    {
        public CompanyCtor(
            Guid id, 
            string name, 
            string description, 
            DateTime dateCreate, 
            DateTime dateUpdate) 
            : base(id, name, description, dateCreate, dateUpdate)
        {}
    }
}
