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
        Result<bool> Approve(Guid emlpoyerId, string feedback);

        /// <summary>
        /// Отказ
        /// </summary>
        /// <param name="emlpoyerId">Индетификатор сотрудника</param>
        /// <param name="feedback">Сообщение, отзыв о соискателе</param>
        Result<bool> Reject(Guid emlpoyerId, string feedback);

        /// <summary>
        /// Отменить решение
        /// </summary>
        /// <param name="emlpoyerId">Индетификатор сотрудника</param>
        Result<bool> Restart(Guid emlpoyerId);
    }
}
