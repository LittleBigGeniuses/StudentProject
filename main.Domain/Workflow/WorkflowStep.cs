using main.domain.Common;
using main.domain.Workflow.Enum;
using main.domain.WorkflowTemplate;

namespace main.domain.Workflow
{

    /// <summary>
    /// Шаг рабочего процесса
    /// </summary>
    public class WorkflowStep : BaseEntity
    {
        private WorkflowStep(Guid candidateId, int number, string description, Guid? employeeId, Guid? roleId)
        {
            CandidateId = candidateId;
            Number = number;
            Description = description;
            EmployeeId = employeeId;
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
        public Guid? EmployeeId { get; private set; }

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

            if (stepTemplate.EmployeeId is null && stepTemplate.RoleId is null)
            {
                return Result<WorkflowStep>.Failure("У шага должна быть привязка к конкретногому сотруднику или должности");
            }

            var step = new WorkflowStep(candidateId, stepTemplate.Number, stepTemplate.Description, stepTemplate.EmployeeId, stepTemplate.RoleId);

            return Result<WorkflowStep>.Success(step);
        }

        /// <summary>
        /// Одобрение
        /// </summary>
        /// <param name="employee">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public Result<bool> Approve(Employee.Employee employee, string? feedback)
        {
            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Шаг завершен");
            }

            Status = Status.Approved;
            Feedback = feedback;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Отказ
        /// </summary>
        /// <param name="employee">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public Result<bool> Reject(Employee.Employee employee, string? feedback)
        {
            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Шаг завершен");
            }

            Status = Status.Rejected;
            Feedback = feedback;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Откат статуса шага
        /// </summary>
        /// <param name="employee">Идентификатор сотрудника</param>
        public Result<bool> Restart(Employee.Employee employee)
        {
            Status = Status.Expectation;
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }
    }
}