using main.domain.Common;

namespace main.domain.Company
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
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Создание новой компании
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <returns></returns>
        public static Result<Company> Create(string name, string description)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Result<Company>.Failure("Наименование компании не может быть пустым");
            }

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
            if (name is not null || !string.IsNullOrEmpty(name))
            {
                if (name.Trim().Length < MinLengthName)
                {
                    return Result<bool>.Failure($"Длина наименование шаблона не может быть меньше {MinLengthName}");
                }

                Name = name.Trim();
            }

            if (description is not null || !string.IsNullOrEmpty(description))
            {
                Description = description.Trim();
            }

            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(true);
        }
    }
}