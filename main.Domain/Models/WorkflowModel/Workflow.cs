using main.domain.Common;
using main.domain.Enum;
using main.domain.Models.WorkflowTemplateModel;
using System.ComponentModel.Design;
using System.Xml.Linq;

namespace main.domain.Models.WorkflowModel
{
    /// <summary>
    /// Сущность Workflow для интервью в компанию
    /// </summary>
    public class Workflow :  BaseEntity, IStatus
    {
        /// <summary>
        /// Минимальное значение длины наименования
        /// </summary>
        public const int MinLengthName = 5;

        /// <summary>
        /// Приватный список шагов, для безопасного взаимодействия
        /// </summary>
        private readonly List<WorkflowStep> _steps = new();
        private Workflow(string name, string description, long authorId, long candidateId, long templateId, long companyId)
        {
            Name = name;
            Description = description;
            AuthorId = authorId;
            CandidateId = candidateId;
            TemplateId = templateId;
            CompanyId = companyId;
            DateCreate = DateTime.Now;
            DateUpdate = DateTime.Now;
        }

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
        public long TemplateId { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника, создавшего рабочий процесс
        /// </summary>
        public long AuthorId { get; private set; }

        /// <summary>
        /// Идентификатор кандидата
        /// </summary>
        public long CandidateId { get; private set; }

        /// <summary>
        /// Идентификатор компании, которой принадллежит рабочий процесс
        /// </summary>
        public long CompanyId { get; private set; }

        /// <summary>
        /// Статус информирующий о положениее рабочего процесса
        /// </summary>
        public Status Status { get; private set; } = Status.Expectation;

        /// <summary>
        /// Безопасный досуп к коллекции шагов
        /// </summary>
        public IReadOnlyCollection<WorkflowStep> Steps => _steps;

        /// <summary>
        /// Создание нового рабочего процесса
        /// </summary>
        /// <param name="authorId">Идентификатор сотрудника, создаюшего рабочий процесс</param>
        /// <param name="candidateId">Идентификатор кандидата</param>
        /// <param name="template">Сущность шаблона</param>
        /// <returns>Результат создания</returns>
        public static Result<Workflow> Create(long authorId, long candidateId, WorkflowTemplate template)
        {
            if (template.Name.Trim().Length < MinLengthName)
            {
                return Result<Workflow>.Failure($"Длина наименование не может быть меньше {MinLengthName}");
            }

            if (authorId <= 0)
            {
                return Result<Workflow>.Failure($"{authorId} - некорректный идентификатор сотрудника");
            }

            if (candidateId <= 0)
            {
                return Result<Workflow>.Failure($"{candidateId} - некорректный идентификатор кандидата");
            }

            if (template is null)
            {
                return Result<Workflow>.Failure($"{nameof(template)} - не может быть пустым");
            }

            var steps = new List<WorkflowStep>();

            var workflow = new Workflow(template.Name, template.Description, authorId, candidateId, template.Id, template.CompanyId);

            foreach (var stepTemplate in template.Steps)
            {
                var stepResult = WorkflowStep.Create(candidateId, stepTemplate, workflow);

                if (stepResult.IsFailure)
                {
                    return Result<Workflow>.Failure($"Не возможен перенос шаблона шага: {stepResult.FailureMessage}");
                }

                var step = stepResult.Data;

                steps.Add(step);
            }

            workflow.AddSteps(steps);

            return Result<Workflow>.Success(workflow);    
        }

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
                if (String.IsNullOrEmpty(name))
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

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Одобрение кандидата
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв сотрудника о кандидате</param>
        public void Approve(long emlpoyerId, string feedback)
        {
            Status = Status.Approved;
            Feedback = feedback;
        }

        /// <summary>
        /// Отказ кандидату
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        /// <param name="feedback">Отзыв сотрудника о кандидате</param>
        public void Reject(long emlpoyerId, string feedback)
        {
            Status = Status.Rejected;
            Feedback = feedback;
        }

        /// <summary>
        /// Возвращение актуальности рабочему процессу
        /// </summary>
        /// <param name="emlpoyerId">Идентификатор сотрудника</param>
        public void Restart(long emlpoyerId)
        {
            Status = Status.Expectation;

            foreach (var step in _steps)
            {
                step.Restart(emlpoyerId);
            }
        }

        /// <summary>
        /// Добавление отзыва (в случае, если workflow был отклонен или одобрен после проверки статусов шагов)
        /// </summary>
        /// <param name="emlpoyerId"></param>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public Result<bool> AddFeedback(long emlpoyerId, string feedback)
        {
            if (String.IsNullOrEmpty(feedback))
            {
                return Result<bool>.Failure("Отзыв не может быть пустым");
            }

            if (emlpoyerId <= 0)
            {
                return Result<bool>.Failure($"{emlpoyerId} - некорректное значение идентификатора сотрудника");
            }

            Feedback = feedback;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Обновление списка шагов (вызывается при создание нового экземпляра рабочего процессса)
        /// </summary>
        /// <param name="workflowSteps">Список шагов</param>
        /// <returns></returns>
        internal Result<bool> AddSteps(List<WorkflowStep>? workflowSteps)
        {
            _steps.Clear();
            _steps.AddRange(workflowSteps);

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Обновление стасуса, относительно статуса шагов
        /// </summary>
        internal void UpdateStatus()
        {
            if (_steps.Any(item => item.Status == Status.Rejected))
            {
                Status = Status.Rejected;
            }
            else if (_steps.All(item => item.Status == Status.Approved))
            {
                Status = Status.Approved;
            }
            else
            {
                Status = Status.Active;
            }
        }
    }
}
