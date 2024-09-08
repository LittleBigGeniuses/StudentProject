using main.domain.Common;
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
        public bool UpdateName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return false;
            }

            Name = name;

            return true;
        }

        /// <summary>
        /// Создание новой сущности соискателя с валидация данных
        /// </summary>
        /// <param name="name">ФИО</param>
        /// <returns>Сущность соискателя</returns>
        public static Condidate? Create(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return null;
            }

            var condidaate = new Condidate(name);

            return condidaate;
        }
    }
}
