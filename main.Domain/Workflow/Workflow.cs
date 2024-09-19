using main.domain.Common;
using main.domain.Employee;
using main.domain.Workflow.Enum;
using main.domain.WorkflowTemplate;

namespace main.domain.Workflow
{
    /// <summary>
    /// Сущность Workflow для интервью в компанию
    /// </summary>
    public class Workflow : BaseEntity
    {
        /// <summary>
        /// Минимальное значение длины наименования
        /// </summary>
        public const int MinLengthName = 5;

        /// <summary>
        /// Приватный список шагов, для безопасного взаимодействия
        /// </summary>
        private static readonly List<WorkflowStep> _steps = new();
        private Workflow(string name, string description, Guid authorId, Guid candidateId, Guid templateId, Guid companyId)
        {
            Name = name;
            Description = description;
            AuthorId = authorId;
            CandidateId = candidateId;
            TemplateId = templateId;
            CompanyId = companyId;
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
        public Status Status => _steps.Any(s => s.Status == Status.Rejected) ? Status.Rejected
                                 : _steps.All(s => s.Status == Status.Approved) ? Status.Approved
                                 : Status.Expectation;



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

            var workflow = new Workflow(template.Name, template.Description, authorId, candidateId, template.Id, template.CompanyId);

            foreach (var stepTemplate in template.Steps)
            {
                var stepResult = WorkflowStep.Create(candidateId, stepTemplate);

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
        /// Возвращение актуальности рабочему процессу
        /// </summary>
        /// <param name="employee">Сущность сотрудника</param>
        public Result<bool> Restart(Employee.Employee employee)
        {
            if (employee is null)
            {
                return Result<bool>.Failure("Сущность сотрдуника не может быть Null");
            }

            foreach (var step in _steps)
            {
                step.Restart(employee);
            }

            DateUpdate = DateTime.Now;

            return Result<bool>.Success(false);
        }

        /// <summary>
        /// Обновление списка шагов (вызывается при создание нового экземпляра рабочего процессса)
        /// </summary>
        /// <param name="workflowSteps">Список шагов</param>
        /// <returns></returns>
        private Result<bool> AddSteps(List<WorkflowStep>? workflowSteps)
        {
            _steps.Clear();
            _steps.AddRange(workflowSteps);

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Одобрение текущего шага
        /// </summary>
        /// <param name="employee">Сущность одобрившего сотрудника</param>
        /// <param name="feedback">Отзыв</param>
        /// <exception cref="Exception"></exception>
        public Result<bool> Approve(Employee.Employee employee, string? feedback)
        {
            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            if (employee is null)
            {
                return Result<bool>.Failure("Сущность сотрдуника не может быть Null");
            }

            var step = Steps.OrderBy(x => x.Number)
                .First(x => x.Status == Status.Expectation);
            step.Approve(employee, feedback);
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Отклонение текущего шага
        /// </summary>
        /// <param name="employee">Сущность отклонившего сотрудника</param>
        /// <param name="feedback">Отзыв</param>
        /// <exception cref="Exception"></exception>
        public Result<bool> Reject(Employee.Employee employee, string? feedback)
        {
            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            if (employee is null)
            {
                return Result<bool>.Failure("Сущность сотрдуника не может быть Null");
            }


            var step = Steps.OrderBy(x => x.Number)
                .First(x => x.Status == Status.Expectation);
            step.Reject(employee, feedback);
            return Result<bool>.Success(true);
        }
    }
}
