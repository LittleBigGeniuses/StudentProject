using main.domain.Common;

namespace main.domain.Company
{
    /// <summary>
    /// Класс компании
    /// </summary>
    public class Company : BaseEntity
    {
        public const int MinLengthName = 5;
        private Company(string name, string description)
        {
            Name = name;
            Description = description;
        }
        /// <summary>
        /// Название компании
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Описание компании
        /// </summary>
        public string Description { get; private set; }

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

            var company = new Company(name, description);

            return Result<Company>.Success(company);
        }

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

            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }
    }
}