using Main.Domain.Common;

namespace Main.Domain.CompanyDomain
{
    /// <summary>
    /// Класс компании
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Минимальная длина наименования компании
        /// </summary>
        public const int MinLengthName = 5;
        private Company(
            Guid id, 
            string name, 
            string description, 
            DateTime dateCreate, 
            DateTime dateUpdate)
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

            Id = id;
            Name = name;
            Description = description;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
        }

        /// <summary>
        /// Создание новой компании
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <returns></returns>
        public static Result<Company> Create(string name, string description)
        {
            if (name.Trim().Length < MinLengthName)
            {
                return Result<Company>.Failure($"Длина наименование шаблона не может быть меньше {MinLengthName}");
            }

            var company = new Company(
                Guid.NewGuid(), 
                name, 
                description, 
                DateTime.UtcNow, 
                DateTime.UtcNow);

            return Result<Company>.Success(company);
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime DateUpdate { get; private set; }
        /// <summary>
        /// Название компании
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Описание компании
        /// </summary>
        public string Description { get; private set; }


        /// <summary>
        /// Обновление данных компании
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <returns></returns>
        public Result<bool> UpdateInfo(string? name, string? description)
        {
            var isChange = false;

            if (name is not null && !string.IsNullOrEmpty(name))
            {
                if (name.Trim().Length < MinLengthName)
                {
                    return Result<bool>.Failure($"Длина наименование шаблона не может быть меньше {MinLengthName}");
                }

                if (name.Trim() != Name)
                {
                    Name = name.Trim();
                    isChange = true;
                }
               
            }

            if (description is not null && !string.IsNullOrEmpty(description))
            {
                if (description.Trim() != Description)
                {
                    Description = description.Trim();
                    isChange = true;
                }             
            }

            if (isChange)
            {
                DateUpdate = DateTime.UtcNow;
            }

            return Result<bool>.Success(true);
        }
    }
}