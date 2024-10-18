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
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"{id} - некорректный идентификатор Компании");
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Наименование компании не может быть пустым");
            }

            if (String.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException("Описание компании не может быть пустым");
            }

            if (dateCreate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата создания не может быть дефолтной.");
            }

            if (dateUpdate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата обновления не может быть дефолтной.");
            }

            if (name.Trim().Length < MinLengthName)
            {
                throw new ArgumentException($"Длина наименования компании не может быть меньше {MinLengthName}");
            }

            Id = id;
            Name = name;
            Description = description;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
        }

        public Guid Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DateCreate { get; }
        public DateTime DateUpdate { get; private set; }
    }
}
