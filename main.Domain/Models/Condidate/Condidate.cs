﻿using main.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Models.Condidate
{
    /// <summary>
    /// Сущность соискателя
    /// </summary>
    public class Condidate : BaseEntity
    {
        const int MinLengthName = 5;
        private Condidate(string name)
        {
            Name = name;
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
            if (String.IsNullOrEmpty(name))
            {
                return Result<bool>.Failure("ФИО сотрудника не может быть пустым");
            }

            Name = name;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true); ;
        }

        /// <summary>
        /// Создание новой сущности соискателя с валидация данных
        /// </summary>
        /// <param name="name">ФИО</param>
        /// <returns>Сущность соискателя</returns>
        public static Result<Condidate> Create(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Result<Condidate>.Failure("ФИО соискателя не может быть пустым");
            }

            if (name.Trim().Length < MinLengthName)
            {
                return Result<Condidate>.Failure($"Длина ФИО соискателя не может быть меньше {MinLengthName}");
            }

            var condidaate = new Condidate(name);

            return Result<Condidate>.Success(condidaate);
        }
    }
}
