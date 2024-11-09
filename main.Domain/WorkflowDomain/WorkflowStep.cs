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
        protected WorkflowStep(
            Guid candidateId, 
            int number, 
            string? feedback,
            string? lastFeedback,
            string description,
            Guid? employeeId,
            Guid? lastEmployeeId,
            Guid? roleId, 
            DateTime dateCreate, 
            DateTime dateUpdate,
            Status status,
            DateTime? restartDate)
        {
            if (candidateId == Guid.Empty)
            {
                throw new ArgumentNullException($"{candidateId} - некорректный идентификатор кандидата");
            }

            if (number < 1)
            {
                throw new ArgumentOutOfRangeException("Некорректный номер шага процесса");
            }

            if (string.IsNullOrEmpty(description))
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
            Feedback = feedback;
            LastFeedback = lastFeedback;
            Description = description;
            EmployeeId = employeeId;
            LastEmployeeId = lastEmployeeId;
            RoleId = roleId;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
            Status = status;
            RestartDate = restartDate;
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
                return Result<WorkflowStep>.Failure("Шаблон шага не может быть неопределен");
            }

            if (stepTemplate.EmployeeId is null && stepTemplate.RoleId is null)
            {
                return Result<WorkflowStep>.Failure("У шага должна быть привязка к конкретному сотруднику или должности");
            }

            var step = new WorkflowStep(candidateId, 
                                        stepTemplate.Number, 
                                        null,
                                        null,
                                        stepTemplate.Description,
                                        stepTemplate.EmployeeId,
                                        null,
                                        stepTemplate.RoleId, 
                                        DateTime.UtcNow, 
                                        DateTime.UtcNow, 
                                        Status.Expectation,  
                                        null);

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
        public string? Feedback { get; private set; }

        /// <summary>
        /// Отзыв сотрудника по шагу (если он был перезапущенн)
        /// </summary>
        public string? LastFeedback { get; private set; }

        /// <summary>
        /// Описание шага
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника, который будет исполнять шаг
        /// </summary>
        public Guid? EmployeeId { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника, который исполнял шаг до перезагрузки
        /// </summary>
        public Guid? LastEmployeeId { get; private set; }

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
        /// Дата перезапуска шага
        /// </summary>
        public DateTime? RestartDate { get; private set; }

        /// <summary>
        /// Одобрение
        /// </summary>
        /// <param name="employee">Сотрудник</param>
        /// <param name="feedback">Отзыв по кандидату</param>
        public Result<bool> Approve(Employee employee, string? feedback)
        {
            if (feedback == "test-failed-approve")
            {
                return Result<bool>.Failure("Отработал тестовый провал одобрения шага");
            }

            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (RoleId is not null 
                && RoleId != Guid.Empty 
                && employee.Id != RoleId)
            {
                return Result<bool>.Failure("Должность сотрудника не соответсвет заявленной");
            }

            if (EmployeeId != Guid.Empty)
            {
                if (employee.Id != EmployeeId)
                {
                    return Result<bool>.Failure("Этот сотрудник не имеет полномочий");
                }
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

            if (feedback == "test-failed-reject")
            {
                return Result<bool>.Failure("Отработал тестовый провал отказа шага");
            }

            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (RoleId is not null
                && RoleId != Guid.Empty 
                && employee.Id != RoleId)
            {
                return Result<bool>.Failure("Должность сотрудника не соответсвует заявленной");
            }

            if (EmployeeId != Guid.Empty)
            {
                if (employee.Id != EmployeeId)
                {
                    return Result<bool>.Failure("Этот сотрудник не имеет полномочий");
                }
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
        /// Перезагрузка шага из workflow
        /// </summary>
        internal void Restart()
        { 
            //Возвращаем шаг в активное состояние
            Status = Status.Expectation;
            //Сохранение прошлого фидбека
            LastFeedback = Feedback;
            //Обнуление текущего фидбека
            Feedback = null;
            //Сохранение идентификатора прошлого исполнителя шага
            LastEmployeeId = EmployeeId;
            //Обнуление текущего исполнителя
            EmployeeId = null;
            //Сохранение даты рестарта
            RestartDate = DateTime.UtcNow;
            //Обновление даты изменения объекта
            DateUpdate = DateTime.UtcNow;
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

            #region проверки для тестов
            if (employee.Name == "test-name-for-failed-result")
            {
                return Result<bool>.Failure("Отработал тестовый провал назначения сотрудника");
            }
            #endregion

            var isChange = false;

            if (employee.Id != EmployeeId)
            {
                EmployeeId = employee.Id;
                RoleId = null;
                isChange = true;
            }

            if (isChange)
            {
                DateUpdate = DateTime.UtcNow;
            }

            return Result<bool>.Success(true);
        }
    }
}