using main.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace main.domain.Models.WorkflowTemplate
{
    /// <summary>
    /// Шаг в шаблоне Workflow
    /// </summary>
    public class WorkflowStepTemplate
    {
        private WorkflowStepTemplate(int number, string description, long? eployerId, long? roleId, WorkflowTemplate workflowTemplate)
        {
            Number = number;
            Description = description;
            EployerId = eployerId;
            RoleId = roleId;
            WorkflowTemplate = workflowTemplate;
            WorkflowTemplateId = workflowTemplate.Id;
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника, который должен будет исполнять шаг
        /// </summary>
        public long? EployerId { get; private set; }

        /// <summary>
        /// Идентификатор должности, которая должна будет исполнять шаг
        /// </summary>
        public long? RoleId { get; private set; }

        /// <summary>
        /// Сущность шаблона, которому принадлежит шаг
        /// </summary>
        public WorkflowTemplate WorkflowTemplate { get; private set; }

        /// <summary>
        /// Идентификатор шаблона Wokflow, которому принадлежит шаг
        /// </summary>
        public long WorkflowTemplateId { get; private set; }

        /// <summary>
        /// Создание нового шага
        /// </summary>
        /// <param name="number">Порядковый номер</param>
        /// <param name="description">Описание</param>
        /// <param name="eployerId">Идентификатор сотрудника</param>
        /// <param name="roleId">Идентификатор роли</param>
        /// <param name="workflowTemplate">Сущность шаблона</param>
        /// <returns></returns>
        internal static Result<WorkflowStepTemplate> Create(int number, string description, long? eployerId, long? roleId, WorkflowTemplate workflowTemplate)
        {
            if (number <= 0)
            {
                return Result<WorkflowStepTemplate>.Failure($"{number} - некорректное значение для номера шага");
            }

            if (eployerId is null && roleId is null)
            {
                return Result<WorkflowStepTemplate>.Failure($"{eployerId} or {roleId}: У шага должна быть привязка к конкретногому сотруднику или должности");
            }

            if (eployerId is not null && eployerId <= 0)
            {
                return Result<WorkflowStepTemplate>.Failure($"{eployerId} - некорректное значение для идентификатора сотрудника в шаге");
            }

            if (roleId is not null && eployerId <= 0)
            {
                return Result<WorkflowStepTemplate>.Failure($"{roleId} - некорректное значение для идентификатора должности в шаге");
            }

            if (workflowTemplate is null)
            {
                return Result<WorkflowStepTemplate>.Failure($"{nameof(workflowTemplate)} не может быть null");
            }

            var stepTemplate = new WorkflowStepTemplate(number, description, eployerId, roleId, workflowTemplate);

            return Result<WorkflowStepTemplate>.Success(stepTemplate);  
        }

        /// <summary>
        /// Обновление данных шага
        /// </summary>
        /// <param name="description">Описание</param>
        /// <returns></returns>
        public Result<bool> UpdateInfo(string description)
        {

            Description = description.Trim();
            
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Обновление номера
        /// </summary>
        /// <param name="number">Новый номер</param>
        /// <returns></returns>
        public Result<bool> UpdateNumber(int number)
        {
            if (number <= 0)
            {
                return Result<bool>.Failure($"{number} - некорректное значение для номера шага");
            }

            Number = number;

            return Result<bool>.Success(true);
        }



    }
}
