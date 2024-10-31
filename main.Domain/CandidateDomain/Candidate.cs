using Main.Domain.Common;
using Main.Domain.WorkflowDomain;

namespace Main.Domain.CandidateDomain
{
    /// <summary>
    /// Сущность соискателя
    /// </summary>
    public class Candidate
    {
        /// <summary>
        /// Минимальная длина наименования кандидата
        /// </summary>
        public const int MinLengthName = 5;

        protected Candidate(
            Guid id, 
            string name, 
            DateTime dateCreate, 
            DateTime dateUpdate)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"{id} - некорректный идентификатор Кандидата");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("ФИО соискателя не может быть пустым");
            }

            if (dateCreate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата создания не может быть дефолтной");
            }

            if (dateUpdate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата обновления не может быть дефолтной");
            }

            if (name.Trim().Length < MinLengthName)
            {
                throw new ArgumentException($"Длина ФИО соискателя не может быть меньше {MinLengthName}");
            }

            Id = id;
            Name = name;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
        }

        /// <summary>
        /// Создание новой сущности соискателя с валидация данных
        /// </summary>
        /// <param name="name">ФИО</param>
        /// <returns>Сущность соискателя</returns>
        public static Result<Candidate> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result<Candidate>.Failure("ФИО соискателя не может быть пустым");
            }

            if (name.Trim().Length < MinLengthName)
            {
                return Result<Candidate>.Failure($"Длина ФИО соискателя не может быть меньше {MinLengthName}");
            }

            var candidate = new Candidate(
                Guid.NewGuid(), 
                name, 
                DateTime.UtcNow,
                DateTime.UtcNow);

            return Result<Candidate>.Success(candidate);
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
        /// ФИО соискателя
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Обновить ФИО
        /// </summary>
        /// <param name="name">ФИО</param>
        /// <returns>Результат обновления (bool)</returns>
        public Result<bool> UpdateName(string name)
        {          
            if (string.IsNullOrEmpty(name))
            {
                return Result<bool>.Failure("ФИО сотрудника не может быть пустым");
            }

            if (name.Trim().Length < MinLengthName)
            {
                return Result<bool>.Failure($"Длина ФИО соискателя не может быть меньше {MinLengthName}");
            }

            var isChanged = false;

            if (name.Trim() != Name)
            {
                Name = name.Trim();
                isChanged = true;
            }

            if (isChanged) 
            {
                DateUpdate = DateTime.UtcNow;
            }

            return Result<bool>.Success(true);
        }
    }
}