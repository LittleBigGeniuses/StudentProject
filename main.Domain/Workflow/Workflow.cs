﻿using main.domain.Common;
using main.domain.Employee;
using main.domain.Workflow.Enum;
using main.domain.WorkflowTemplate;

namespace main.domain.Workflow
{
    /// <summary>
    /// Сущность Workflow для интервью в компанию
    /// </summary>
    public class Workflow
    {
        /// <summary>
        /// Минимальное значение длины наименования
        /// </summary>
        public const int MinLengthName = 5;
        private Workflow(
            Guid id, 
            string name, 
            string description, 
            IReadOnlyCollection<WorkflowStep> steps, 
            Guid authorId, Guid candidateId, 
            Guid templateId, Guid companyId, 
            DateTime dateCreate, 
            DateTime dateUpdate)
        {
            Id = id;
            Name = name;
            Description = description;
            Steps = steps;
            AuthorId = authorId;
            CandidateId = candidateId;
            TemplateId = templateId;
            CompanyId = companyId;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
        }

        /// <summary>
        /// Создание нового рабочего процесса
        /// </summary>
        /// <param name="authorId">Идентификатор сотрудника, создаюшего рабочий процесс</param>
        /// <param name="candidateId">Идентификатор кандидата</param>
        /// <param name="template">Сущность шаблона</param>
        /// <returns>Результат создания</returns>
        public static Result<Workflow> Create(Guid authorId, Guid candidateId, WorkflowTemplate.WorkflowTemplate template)
        {
            if (template.Name.Trim().Length < MinLengthName)
            {
                return Result<Workflow>.Failure($"Длина наименование не может быть меньше {MinLengthName}");
            }

            if (authorId == Guid.Empty)
            {
                return Result<Workflow>.Failure($"{authorId} - некорректный идентификатор сотрудника");
            }

            if (candidateId == Guid.Empty)
            {
                return Result<Workflow>.Failure($"{candidateId} - некорректный идентификатор кандидата");
            }

            if (template is null)
            {
                return Result<Workflow>.Failure($"{nameof(template)} - не может быть пустым");
            }

            var steps = new List<WorkflowStep>();

            var workflow = new Workflow(
                Guid.NewGuid(), 
                template.Name, 
                template.Description, 
                steps, 
                authorId, 
                candidateId, 
                template.Id, 
                template.CompanyId, 
                DateTime.UtcNow, 
                DateTime.UtcNow);

            return Result<Workflow>.Success(workflow);
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime DateUpdate { get; private set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Отчет сотрудника
        /// </summary>
        public string? Feedback { get; private set; } = null;

        /// <summary>
        /// Идентификатор шаблона, на основе которого был создан рабочий процесс
        /// </summary>
        public Guid TemplateId { get; }

        /// <summary>
        /// Идентификатор сотрудника, создавшего рабочий процесс
        /// </summary>
        public Guid AuthorId { get; }

        /// <summary>
        /// Идентификатор кандидата
        /// </summary>
        public Guid CandidateId { get; }

        /// <summary>
        /// Идентификатор компании, которой принадллежит рабочий процесс
        /// </summary>
        public Guid CompanyId { get; }

        /// <summary>
        /// Статус информирующий о положениее рабочего процесса
        /// </summary>
        public Status Status => Steps.Any(s => s.Status == Status.Rejected) ? Status.Rejected
                                 : Steps.All(s => s.Status == Status.Approved) ? Status.Approved
                                 : Status.Expectation;

        /// <summary>
        /// Терминальность рабочего процесса
        /// </summary>
        public bool IsTerminal { get; private set; } = false;


        /// <summary>
        /// Безопасный досуп к коллекции шагов
        /// </summary>
        public IReadOnlyCollection<WorkflowStep> Steps;


        /// <summary>
        /// Обновление основной информации о рабочем процесса
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <returns></returns>
        public Result<bool> UpdateInfo(string? name, string? description)
        {
            if (name is not null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Result<bool>.Failure("Наименование шаблона не может быть пустым");
                }

                if (name.Trim().Length < MinLengthName)
                {
                    return Result<bool>.Failure($"Длина наименование шаблона не может быть меньше {MinLengthName}");
                }

                Name = name.Trim();
            }

            if (description is not null)
            {
                Description = description.Trim();
            }

            DateUpdate = DateTime.Now;
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Одобрение кандидата
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв сотрудника о кандидате</param>
        public Result<bool> Approve(Guid emlpoyerId, string feedback)
        {
            if (emlpoyerId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            if (IsTerminal)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            if (Status != Status.Approved)
            {
                return Result<bool>.Failure("Отклоненный рабочий процесс, не может быть одобрен");
            }

            IsTerminal = true;
            Feedback = feedback;

            DateUpdate = DateTime.Now;
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Отказ кандидату
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв сотрудника о кандидате</param>
        public Result<bool> Reject(Guid emlpoyerId, string feedback)
        {
            if (emlpoyerId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            if (IsTerminal)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            IsTerminal = true;
            Feedback = feedback;

            DateUpdate = DateTime.Now;
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Возвращение актуальности рабочему процессу
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        public Result<bool> Restart(Guid emlpoyerId)
        {
            if (emlpoyerId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            IsTerminal = false;

            foreach (var step in Steps)
            {
                step.Restart(emlpoyerId);
            }

            DateUpdate = DateTime.Now;

            return Result<bool>.Success(false);
        }

        /// <summary>
        /// Назначает нового сотрудника на текущий шаг процесса
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника, назначенного на текущий этап процесса</param>
        /// <returns></returns>
        public Result<bool> SetEmployee(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            Steps.OrderBy(x => x.Number).First(x => x.Status == Status.Expectation).AddEmployeeId(employeeId);
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Назначает нового сотрудника на указанный номер шага процесса
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника, назначенного на указанный этап процесса</param>
        /// <param name="number">Номер процесса на который будет назначен сотрудник</param>
        /// <returns></returns>
        public Result<bool> SetEmployee(Guid employeeId, int number)
        {
            if (employeeId == Guid.Empty)
            {
                return Result<bool>.Failure("Некорректный идентификатор сотрудника");
            }

            foreach (var step in Steps)
            {
                if (step.Number == number)
                {
                    step.AddEmployeeId(employeeId);
                }
            }

            return Result<bool>.Success(true);
        }
    }
}
