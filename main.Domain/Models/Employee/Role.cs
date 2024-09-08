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
        private const int MinLengthName = 3;

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
        /// Идентификатор компании, в которой принадлежит должность
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
        public static Role? Create(string name, long companyId)
        {
            if (String.IsNullOrEmpty(name))
            {
                return null;
            }

            if (name.Length < MinLengthName)
            {
                return null;
            }

            var role = new Role(name, companyId);

            return role;
        }

    }
}
