using main.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Models.Employee
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

        private Role(string name, long companyId)
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
        public long CompanyId { get; private set; }

        /// <summary>
        /// Список сотрудников, которым принадлежит должность
        /// </summary>
        List<Employee> Employees { get; set; } = new();

        /// <summary>
        /// Создание новой должностит
        /// </summary>
        /// <param name="name"> Наименование должности</param>
        /// <param name="companyId"> Идентификатор компании</param>
        /// <returns>Возвращает сущность должноти</returns>
        public static Result<Role> Create(string name, long companyId)
        {
            if (String.IsNullOrEmpty(name))
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
            if (String.IsNullOrEmpty(name))
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
