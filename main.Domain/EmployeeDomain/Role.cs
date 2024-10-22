using Main.Domain.Common;

namespace Main.Domain.EmployeeDomain
{
    /// <summary>
    /// Класс должности сотрудника в компании
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Минимально допутимая длина наименования роли
        /// </summary>
        public const int MinLengthName = 3;

        protected Role(
            Guid id, 
            string name, 
            Guid companyId, 
            DateTime dateCreate, 
            DateTime dateUpdate)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"{id} - некорректный идентификатор должности");
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Наименование должности не может быть пустым");
            }

            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException($"{companyId} - некорректный идентификатор компании");
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
                throw new ArgumentException($"Длина наименования должности не может быть меньше {MinLengthName}");
            }

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

            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException($"{companyId} - некорректный идентификатор компании");
            }

            if (name.Trim().Length < MinLengthName)
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

            if (name.Trim() != Name)
            {
                Name = name.Trim();
                DateUpdate = DateTime.UtcNow;
            }

            return Result<bool>.Success(true);
        }

    }
}