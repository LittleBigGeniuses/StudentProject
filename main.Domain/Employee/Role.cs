using main.domain.Common;

namespace main.domain.Employee
{
    /// <summary>
    /// Класс должности сотрудника в компании
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Минимально допутимая длина наименования роли
        /// </summary>
        const int MinLengthName = 3;

        private Role(string name, Guid companyId)
        {
            Name = name;
            CompanyId = companyId;
        }

        /// <summary>
        /// Наименование должность
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Идентификатор компании, к которой относится должность
        /// </summary>
        public Guid CompanyId { get; }

        /// <summary>
        /// Создание новой должностит
        /// </summary>
        /// <param name="name"> Наименование должности</param>
        /// <param name="companyId"> Идентификатор компании</param>
        /// <returns>Возвращает сущность должноти</returns>
        public static Result<Role> Create(string name, Guid companyId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result<Role>.Failure("Наименование должности не может быть пустым");
            }

            if (name.Length < MinLengthName)
            {
                return Result<Role>.Failure($"Длина наименования должности не может быть меньше {MinLengthName}");
            }

            var role = new Role(name, companyId);

            return Result<Role>.Success(role);
        }

        /// <summary>
        /// Обновление наименования должности
        /// </summary>
        /// <param name="name">Новое наименование</param>
        /// <returns>Успешность выполнения операции</returns>
        public Result<bool> UpdateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result<bool>.Failure("Наименование должности не может быть пустым");
            }

            if (name.Trim().Length < MinLengthName)
            {
                return Result<bool>.Failure($"Длина наименование должности не может быть меньше {MinLengthName}");
            }

            Name = name;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }

    }
}