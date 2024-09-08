using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Common
{
    /// <summary>
    /// Интерфейс содержащий необходимые методы для обновления статуса сущности.
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        /// Успешное прохождение
        /// </summary>
        /// <param name="emlpoyerId">Индетификатор сотрудника</param>
        /// <param name="feedback">Сообщение, отзыв о соискателе</param>
        void Approve(long emlpoyerId, string feedback);

        /// <summary>
        /// Отказ
        /// </summary>
        /// <param name="emlpoyerId">Индетификатор сотрудника</param>
        /// <param name="feedback">Сообщение, отзыв о соискателе</param>
        void Reject(long emlpoyerId, string feedback);

        /// <summary>
        /// Отменить решение
        /// </summary>
        /// <param name="emlpoyerId">Индетификатор сотрудника</param>
        void Restart(long emlpoyerId);
    }
}
