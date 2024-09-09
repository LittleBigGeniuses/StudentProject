using main.domain.Common;
using main.domain.Enum;
using main.domain.Models.EmployeeModel;
using main.domain.Models.WorkflowTemplateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Models.WorkflowModel
{

    /// <summary>
    /// Шаг рабочего процесса
    /// </summary>
    public class WorkflowStep :  BaseEntity, IStatus
    {
        private WorkflowStep(long candidateId, int number, string description, long? employerId, long? roleId, Workflow workflow)
        {
            CandidateId = candidateId;
            Number = number;
            Description = description;
            EmployerId = employerId;
            RoleId = roleId;
            Workflow = workflow;
            WorkflowId = workflow.Id;
            DateCreate = DateTime.Now;
            DateUpdate = DateTime.Now;
        }

        /// <summary>
        /// Порядковый номер шага
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Отзыв сотрудника по шагу
        /// </summary>
        public string? Feedback { get; private set; } = null;

        /// <summary>
        /// Описание шага
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника, который будет исполнять шаг
        /// </summary>
        public long? EmployerId { get; private set; }

        /// <summary>
        /// Идентификатор роли, которая может исполнить шаг
        /// </summary>
        public long? RoleId { get; private set; }

        /// <summary>
        /// Идентификатор кандидата
        /// </summary>
        public long CandidateId { get; private set; }

        /// <summary>
        /// Сущность рабочего процесса, которой принаждежит шаг
        /// </summary>
        public Workflow Workflow { get; private set; }

        /// <summary>
        /// Идентификатор рабочего процесса, которой принаждежит шаг
        /// </summary>
        public long WorkflowId { get; private set; }

        /// <summary>
        /// Стастус шага
        /// </summary>
        public Status Status { get; private set; } = Status.Expectation;

        /// <summary>
        /// Создание шага
        /// </summary>
        /// <param name="candidateId">Идентификатор кандидата</param>
        /// <param name="stepTemplate">Шаблон, по которому создается шаг</param>
        /// <param name="workflow">Рабочий процесс, которому принадлежит шаг</param>
        /// <returns></returns>
        internal static Result<WorkflowStep> Create(long candidateId, WorkflowStepTemplate stepTemplate, Workflow workflow)
        {
            if (candidateId <= 0)
            {
                return Result<WorkflowStep>.Failure($"{candidateId} - некорректный идентификатор кандидата");
            }

            if (stepTemplate is null)
            {
                return Result<WorkflowStep>.Failure($"{nameof(stepTemplate)} не может быть пустым");
            }

            if (workflow is null)
            {
                return Result<WorkflowStep>.Failure($"{nameof(workflow)} не может быть пустым");
            }

            if (stepTemplate.EployerId is null && stepTemplate.RoleId is null)
            {
                return Result<WorkflowStep>.Failure("У шага должна быть привязка к конкретногому сотруднику или должности");
            }

            var step = new WorkflowStep(candidateId, stepTemplate.Number,stepTemplate.Description, stepTemplate.EployerId, stepTemplate.RoleId, workflow);

            return Result<WorkflowStep>.Success(step);
        }

        /// <summary>
        /// Одобрение
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public void Approve(long emlpoyerId, string? feedback)
        {
            Status = Status.Approved;
            Feedback = feedback;
            Workflow.UpdateStatus();
        }

        /// <summary>
        /// Отказ
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public void Reject(long emlpoyerId, string? feedback)
        {
            Status = Status.Rejected;
            Feedback = feedback;
            Workflow.UpdateStatus();
        }

        /// <summary>
        /// Откат статуса шага
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        public void Restart(long emlpoyerId)
        {
            Status = Status.Expectation;
            Workflow.UpdateStatus();
        }
    }
}
