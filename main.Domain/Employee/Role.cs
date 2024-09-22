using main.domain.Common;

namespace main.domain.Employee
{
    /// <summary>
    /// Класс должности сотрудника в компании
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Минимально допутимая длина наименования роли
        /// </summary>
        const int MinLengthName = 3;

        private Role(
            Guid id, 
            string name, 
            Guid companyId, 
            DateTime dateCreate, 
            DateTime dateUpdate)
        {
            Id = id;
            Name = name;
            CompanyId = companyId;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
        }

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

            var role = new Role(Guid.NewGuid(), name, companyId, DateTime.UtcNow, DateTime.UtcNow);

            return Result<Role>.Success(role);
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
        /// Наименование должность
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Идентификатор компании, к которой относится должность
        /// </summary>
        public Guid CompanyId { get; }


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
            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(true);
        }

    }
}