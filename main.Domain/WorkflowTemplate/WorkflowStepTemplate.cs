using main.domain.Common;
using main.domain.Employee;

namespace main.domain.WorkflowTemplate
{
    /// <summary>
    /// Шаг в шаблоне Workflow
    /// </summary>
    public class WorkflowStepTemplate
    {
        private WorkflowStepTemplate(int number, string description, Guid? eployeeId, Guid? roleId, DateTime dateCreate, DateTime dateUpdate)
        {
            Number = number;
            Description = description;
            EmployeeId = eployeeId;
            RoleId = roleId;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;
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
        /// Описание
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника, который должен будет исполнять шаг
        /// </summary>
        public Guid? EmployeeId { get; private set; }

        /// <summary>
        /// Идентификатор должности, которая должна будет исполнять шаг
        /// </summary>
        public Guid? RoleId { get; private set; }


        /// <summary>
        /// Создание нового шага
        /// </summary>
        /// <param name="number">Порядковый номер</param>
        /// <param name="description">Описание</param>
        /// <param name="eployerId">Идентификатор сотрудника</param>
        /// <param name="roleId">Идентификатор роли</param>
        /// <returns></returns>
        internal static Result<WorkflowStepTemplate> Create(int number, string description, Guid? eployerId, Guid? roleId)
        {
            if (number <= 0)
            {
                return Result<WorkflowStepTemplate>.Failure($"{number} - некорректное значение для номера шага");
            }

            if (eployerId is null && roleId is null)
            {
                return Result<WorkflowStepTemplate>.Failure("У шага должна быть привязка к конкретногому сотруднику или должности");
            }

            if (eployerId is not null && eployerId == Guid.Empty)
            {
                return Result<WorkflowStepTemplate>.Failure($"{eployerId} - некорректное значение для идентификатора сотрудника в шаге");
            }

            if (roleId is not null && roleId == Guid.Empty)
            {
                return Result<WorkflowStepTemplate>.Failure($"{roleId} - некорректное значение для идентификатора должности в шаге");
            }


            var stepTemplate = new WorkflowStepTemplate(number, description, eployerId, roleId, DateTime.UtcNow, DateTime.UtcNow);

            return Result<WorkflowStepTemplate>.Success(stepTemplate);
        }

        /// <summary>
        /// Обновление данных шага
        /// </summary>
        /// <param name="description">Описание</param>
        /// <returns></returns>
        public Result<bool> UpdateInfo(string description)
        {

            Description = description.Trim();
            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Обновление номера
        /// </summary>
        /// <param name="number">Новый номер</param>
        /// <returns></returns>
        public Result<bool> UpdateNumber(int number)
        {
            if (number <= 0)
            {
                return Result<bool>.Failure($"{number} - некорректное значение для номера шага");
            }

            Number = number;

            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Обновление идентификатора должности
        /// </summary>
        /// <param name="roleId">новый идентификатор должности</param>
        /// <returns></returns>
        public Result<bool> UpdateRoleId(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                return Result<bool>.Failure($"{roleId} - некорректный идентификатор должности");
            }

            RoleId = roleId;

            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }
        /// <summary>
        /// Обновление идентификатора работника
        /// </summary>
        /// <param name="employeeId">Новый идентификатор работника</param>
        /// <returns></returns>
        public Result<bool> UpdateEmployeeId(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                return Result<bool>.Failure($"{employeeId} - некорректный идентификатор сотрудника");
            }

            EmployeeId = employeeId;

            DateUpdate = DateTime.Now;

            return Result<bool>.Success(true);
        }
    }
}