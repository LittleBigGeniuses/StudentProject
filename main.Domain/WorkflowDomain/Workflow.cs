using Main.Domain.Common;
using Main.Domain.EmployeeDomain;
using Main.Domain.WorkflowDomain.Enum;
using Main.Domain.WorkflowTemplateDomain;
using System.Text;

namespace Main.Domain.WorkflowDomain
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
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"{id} - некорректный идентификатор Процесса");
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Название процесса не может быть пустым");
            }

            if (String.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException("Описание процесса не может быть пустым");
            }

            if (steps is null)
            {
                throw new ArgumentNullException("Список шагов должен быть определен");
            }

            if (steps.Count <= 0)
            {
                throw new ArgumentException("Список шагов не может быть пустым");
            }

            if (steps.Any(s => s is null))
            {
                throw new ArgumentException("Все шаги в списке должны быть определены");
            }

            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException($"{authorId} - некорректный идентификатор создателя процесса");
            }

            if (candidateId == Guid.Empty)
            {
                throw new ArgumentNullException($"{candidateId} - некорректный идентификатор кандидата");
            }

            if (templateId == Guid.Empty)
            {
                throw new ArgumentNullException($"{templateId} - некорректный идентификатор шаблона процесса");
            }

            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException($"{companyId} - некорректный идентификатор компании");
            }

            if (dateCreate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата создания не может быть дефолтной.");
            }

            if (dateUpdate == DateTime.MinValue)
            {
                throw new ArgumentException("Дата обновления не может быть дефолтной.");
            }

            if (name.Trim().Length < MinLengthName)
            {
                throw new ArgumentException($"Длина наименование процесса не может быть меньше {MinLengthName}");
            }

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
        public static Result<Workflow> Create(Guid authorId, Guid candidateId, WorkflowTemplate template)
        {
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

            if (template.Name.Trim().Length < MinLengthName)
            {
                return Result<Workflow>.Failure($"Длина наименование не может быть меньше {MinLengthName}");
            }

            if (template.Steps.Count == 0)
            {
                return Result<Workflow>.Failure("Предложенный шаблон не содержит шаги");
            }

            var stepsResults = template.Steps
                .Select(s => WorkflowStep.Create(candidateId, s))
                .ToList();

            if (stepsResults.Any(result => result.IsFailure))
            {
                return Result<Workflow>.Failure("Ошибка при создание шагов");
            }

            var steps = stepsResults.Select(r => r.Value)
                .ToList()
                .AsReadOnly();

            if (steps.Any(s => s is null))
            {
                return Result<Workflow>.Failure("Все шаги в списке должны быть определены");
            }

            var workflow = new Workflow(
                Guid.NewGuid(), 
                template.Name, 
                template.Description,
                steps!,
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
            var isChange = false;

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

                if (name != Name)
                {
                    Name = name.Trim();
                    isChange = true;
                }
            }

            if (description is not null)
            {
                if (description != Description)
                {
                    Description = description.Trim();
                    isChange = true;
                }
            }
            
            if (isChange)
            {
                DateUpdate = DateTime.UtcNow;
            }

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Одобрение кандидата
        /// </summary>
        /// <param name="employee">Cотрудник</param>
        /// <param name="feedback">Отзыв сотрудника о кандидате</param>
        public Result<bool> Approve(Employee employee, string feedback)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            if (Status != Status.Approved)
            {
                return Result<bool>.Failure("Отклоненный рабочий процесс, не может быть одобрен");
            }

            var step = Steps
                 .OrderBy(x => x.Number)
                 .First(s => s.Status == Status.Expectation);

            var resultApprove = step.Approve(employee, feedback);

            if (resultApprove.IsFailure)
            {
                return resultApprove;
            }

            DateUpdate = DateTime.UtcNow;
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Отказ кандидату
        /// </summary>
        /// <param name="employee">Cотрудник</param>
        /// <param name="feedback">Отзыв сотрудника о кандидате</param>
        public Result<bool> Reject(Employee employee, string feedback)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure("Рабочий процесс завершен");
            }

            var step = Steps
                .OrderBy(x => x.Number)
                .First(s => s.Status == Status.Expectation);

            var resultReject = step.Reject(employee, feedback);

            if (resultReject.IsFailure)
            {
                return resultReject;
            }

            DateUpdate = DateTime.UtcNow;
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

            foreach (var step in Steps)
            {
                step.Restart(emlpoyerId);
            }

            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(false);
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

            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure($"Рабочий процесс завершен");
            }

            var step = Steps
                .OrderBy(x => x.Number)
                .First(s => s.Status == Status.Expectation);

            var result = step.SetEmployee(employee);

            if (result.IsFailure)
            {
                return result;
            }

            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Назначение сотрудника на указанный шаг
        /// </summary>
        /// <param name="employee">Сотрудник</param>
        /// <param name="numberStep">Номер шага</param>
        /// <returns></returns>
        public Result<bool> SetEmployeeInStep(Employee employee, int numberStep)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }

            if (Status != Status.Expectation)
            {
                return Result<bool>.Failure($"Рабочий процесс завершен");
            }

            var step = Steps
                .FirstOrDefault(s => s.Number == numberStep);

            if (step is null)
            {
                return Result<bool>.Failure($"Шаг с номером {numberStep} не найден");
            }

            if (step.Status != Status.Expectation)
            {
                return Result<bool>.Failure($"Шаг {numberStep} завершен");
            }

            var result = step.SetEmployee(employee);

            if (result.IsFailure)
            {
                return result;
            }

            DateUpdate = DateTime.UtcNow;

            return Result<bool>.Success(true);
        }
    }
}
