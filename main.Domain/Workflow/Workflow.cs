using main.domain.Common;
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
                [], 
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

            DateUpdate = DateTime.UtcNow;
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Одобрение кандидата
        /// </summary>
        /// <param name="employee">Cотрудник</param>
        /// <param name="feedback">Отзыв сотрудника о кандидате</param>
        public Result<bool> Approve(Employee.Employee employee, string feedback)
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
        public Result<bool> Reject(Employee.Employee employee, string feedback)
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
        public Result<bool> SetEmployee(Employee.Employee employee)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
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
        public Result<bool> SetEmployeeInStep(Employee.Employee employee, int numberStep)
        {
            if (employee is null)
            {
                return Result<bool>.Failure($"{nameof(employee)} не может быть пустым");
            }


            var step = Steps
                .FirstOrDefault(s => s.Number == numberStep);

            if (step is null)
            {
                return Result<bool>.Failure($"Шага с номером {numberStep} не найдено");
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
