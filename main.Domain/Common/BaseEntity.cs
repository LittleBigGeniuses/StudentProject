using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Common
{
    /// <summary>
    /// Базовая сущность, хранящая обязательные поля для хранения в БД
    /// </summary>
    public class BaseEntity : IEntityId, IEntityDate
    {
        /// <summary>
        /// Идентификатор в система
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Дата и время обновления
        /// </summary>
        public DateTime DateUpdate { get; set; }

    }
}
