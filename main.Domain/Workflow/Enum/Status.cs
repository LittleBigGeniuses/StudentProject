using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Enum
{
    /// <summary>
    /// Статус объекта
    /// </summary>
    public enum Status
    {

        /// <summary>
        /// Ожидание
        /// </summary>
        Expectation = 0,

        /// <summary>
        /// Одобренно
        /// </summary>
        Approved = 1,

        /// <summary>
        /// Отказано
        /// </summary>
        Rejected = 2
    }
}
