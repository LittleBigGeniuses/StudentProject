using Main.Domain.Common;
using Main.Domain.EmployeeDomain;
using Main.Domain.WorkflowDomain.Enum;
using Main.Domain.WorkflowTemplateDomain;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("main.DomainTest")]

namespace Main.Domain.WorkflowDomain
{

    /// <summary>
    /// Шаг рабочего процесса
    /// </summary>
    public class WorkflowStep
    {
        private WorkflowStep(
            Guid candidateId, 
            int number, string 
            description, 
            Guid? employeeId, 
            Guid? roleId, 
            DateTime dateCreate, 
            DateTime dateUpdate)
        {
            if (candidateId == Guid.Empty)
            {
                throw new ArgumentNullException($"{candidateId} - некорректный идентификатор кандидата");
            }

            if (number < 1)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер шага процесса");
            }

            if (String.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException("Описание шага процесса не может быть пустым");
            }

            if (employeeId is null && roleId is null)
            {
                throw new ArgumentNullException("У шага должна быть привязка к конкретногому сотруднику или должности");
            }

            if (employeeId is not null && employeeId == Guid.Empty)
            {
                throw new ArgumentNullException($"{employeeId} - некорректное значение для идентификатора сотрудника в шаге");
            }

            if (roleId is not null && roleId == Guid.Empty)
            {
                throw new ArgumentNullException($"{roleId} - некорректное значение для идентификатора должности в шаге");
            }

            if (dateCreate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата создания не может быть дефолтной.");
            }

            if (dateUpdate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата обновления не может быть дефолтной.");
            }

            CandidateId = candidateId;
            Number = number;
            Description = description;
            EmployeeId = employeeId;
            RoleId = roleId;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
            Status = Status.Expectation;
        }

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

            var step = new WorkflowStep(candidateId, stepTemplate.Number, stepTemplate.Description, stepTemplate.EmployeeId, stepTemplate.RoleId, DateTime.UtcNow, DateTime.UtcNow);

            return Result<WorkflowStep>.Success(step);
        }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime DateUpdate { get; private set; }

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
        public Status Status { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника длегированного на процесс
        /// </summary>
        public Guid? DelegatedEmployeeId { get; private set; }


        /// <summary>
        /// Одобрение
        /// </summary>
        /// <param name="employee">Сотрудник</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public Result<bool> Approve(Employee employee, string? feedback)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (employee.Id != EmployeeId)
            {
                return Result<bool>.Failure($"У этого шага другой исполнитель");
            }

            if (employee.RoleId != RoleId && employee.Id != EmployeeId)
            {
                return Result<bool>.Failure($"Роль не соответвует требованиям, для исполнения шага");
            }

            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Шаг завершен");
            }

            Status = Status.Approved;
            Feedback = feedback;
            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Отказ
        /// </summary>
        /// <param name="employee">Сотрудник</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public Result<bool> Reject(Employee employee, string? feedback)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (employee.Id != EmployeeId)
            {
                return Result<bool>.Failure($"У этого шага другой исполнитель");
            }

            if (employee.RoleId != RoleId && employee.Id != EmployeeId)
            {
                return Result<bool>.Failure($"Роль не соответвует требованиям, для исполнения шага");
            }

            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Шаг завершен");
            }

            Status = Status.Rejected;
            Feedback = feedback;
            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Откат статуса шага
        /// </summary>
        /// <param name="employerId">Идентификатор сотрудника</param>
        public Result<bool> Restart(Guid employerId)
        {
            if (employerId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            Status = Status.Expectation;
            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Назначение нового сотрудника на шаг
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Result<bool> SetEmployee(Employee employee)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (employee.Id != EmployeeId)
            {
                EmployeeId = employee.Id;
                RoleId = null;
                DateUpdate = DateTime.UtcNow;
            }

            return Result<bool>.Success(true);
        }

        public Result<bool> SetDelegatedEmployee(Employee employee)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (employee.Id != DelegatedEmployeeId)
            {
                DelegatedEmployeeId = employee.Id;
                DateUpdate = DateTime.UtcNow;
            }

            return Result<bool>.Success(true);
        }
    }
}