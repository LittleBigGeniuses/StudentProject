using main.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.CondidateModel
{
    /// <summary>
    /// Сущность соискателя
    /// </summary>
    public class Candidate : BaseEntity
    {
        const int MinLengthName = 5;
        private Candidate(string name)
        {
            Name = name;
            DateCreate = DateTime.Now;
            DateUpdate = DateTime.Now;
        }

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

            Name = name;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
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

            var condidaate = new Candidate(name);

            return Result<Candidate>.Success(condidaate);
        }
    }
}
