using main.domain.Common;
using main.domain.Enum;
using main.domain.WorkflowTemplate;


namespace main.domain.Workflow
{

    /// <summary>
    /// Шаг рабочего процесса
    /// </summary>
    public class WorkflowStep : BaseEntity, IStatus
    {
        private WorkflowStep(Guid candidateId, int number, string description, Guid? employerId, Guid? roleId)
        {
            CandidateId = candidateId;
            Number = number;
            Description = description;
            EmployerId = employerId;
            RoleId = roleId;
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
        public Guid? EmployerId { get; private set; }

        /// <summary>
        /// Идентификатор роли, которая может исполнить шаг
        /// </summary>
        public Guid? RoleId { get; private set; }

        /// <summary>
        /// Идентификатор кандидата
        /// </summary>
        public Guid CandidateId { get; }

        /// <summary>
        /// Стастус шага
        /// </summary>
        public Status Status { get; private set; } = Status.Expectation;

        /// <summary>
        /// Терминальность шага
        /// </summary>
        public bool IsTerminal { get; private set; } = false;

        /// <summary>
        /// Создание шага
        /// </summary>
        /// <param name="candidateId">Идентификатор кандидата</param>
        /// <param name="stepTemplate">Шаблон, по которому создается шаг</param>
        /// <returns></returns>
        internal static Result<WorkflowStep> Create(Guid candidateId, WorkflowStepTemplate stepTemplate)
        {
            if (candidateId == Guid.Empty)
            {
                return Result<WorkflowStep>.Failure($"{candidateId} - некорректный идентификатор кандидата");
            }

            if (stepTemplate is null)
            {
                return Result<WorkflowStep>.Failure($"{nameof(stepTemplate)} не может быть пустым");
            }

            if (stepTemplate.EployerId is null && stepTemplate.RoleId is null)
            {
                return Result<WorkflowStep>.Failure("У шага должна быть привязка к конкретногому сотруднику или должности");
            }

            var step = new WorkflowStep(candidateId, stepTemplate.Number, stepTemplate.Description, stepTemplate.EployerId, stepTemplate.RoleId);

            return Result<WorkflowStep>.Success(step);
        }

        /// <summary>
        /// Одобрение
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public Result<bool> Approve(Guid emlpoyerId, string? feedback)
        {
            if (emlpoyerId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            if (IsTerminal)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            Status = Status.Approved;
            Feedback = feedback;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Отказ
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public Result<bool> Reject(Guid emlpoyerId, string? feedback)
        {
            if (emlpoyerId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            if (IsTerminal)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            Status = Status.Rejected;
            Feedback = feedback;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Откат статуса шага
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        public Result<bool> Restart(Guid emlpoyerId)
        {
            if (emlpoyerId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            Status = Status.Expectation;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }
    }
}