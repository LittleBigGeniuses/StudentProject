using main.domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Models.WorkflowTemplateModel
{
    /// <summary>
    /// Шаблон Workfloy
    /// </summary>
    public class WorkflowTemplate : BaseEntity
    {
        /// <summary>
        /// Константное значение минимальной длины наименования шаблона
        /// </summary>
        public const int MinLengthName = 5;

        /// <summary>
        /// Защищенный список шагов
        /// </summary>
        private readonly List<WorkflowStepTemplate> _steps = new();

        private WorkflowTemplate(string name, string description, long companyId)
        {
            Name = name;
            Description = description;
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
        /// Идентификатор компании, которой принадлежит шаблон
        /// </summary>
        public long CompanyId { get; }

        /// <summary>
        /// Безопасный список, для чтения из вне
        /// </summary>
        public IReadOnlyCollection<WorkflowStepTemplate> Steps => _steps;

        /// <summary>
        /// Метод создание нового шаблона
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <param name="companyId">Идентификатор компании</param>
        /// <returns>Сущность с результатом создания</returns>
        public static Result<WorkflowTemplate> Create(string name, string description, long companyId)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Result<WorkflowTemplate>.Failure("Наименование шаблона не может быть пустым");
            }

            if (name.Trim().Length < MinLengthName)
            {
                return Result<WorkflowTemplate>.Failure($"Длина наименование шаблона не может быть меньше {MinLengthName}");
            }

            if (companyId <= 0)
            {
                return Result<WorkflowTemplate>.Failure($"{companyId} - некорректный идентификатор компании");
            }

            var workflowTemplate = new WorkflowTemplate(name.Trim(), description, companyId);

            return Result<WorkflowTemplate>.Success(workflowTemplate);
        }

        /// <summary>
        /// Метод обновления информации
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <returns>Результат обновления информации</returns>
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
        /// Добавление нового шага
        /// </summary>
        /// <param name="description">Описание</param>
        /// <param name="employerId">Идентификатор сотрудника, исполняемого шаг</param>
        /// <param name="roleId">Идентификатор должности, исполняемой шаг</param>
        /// <returns>Результат добавления шага</returns>
        public Result<bool> AddStep(string description, long? employerId, long? roleId)
        {
            var createStep = WorkflowStepTemplate.Create(_steps.Count + 1, description, employerId, roleId, this);

            if (createStep.IsFailure)
            {
                return Result<bool>.Failure($"Добавление элемента в список провалиловсь: {createStep.FailureMessage}");
            }

            var step = createStep.Data;

            _steps.Add(step);

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Удаление шага
        /// </summary>
        /// <param name="number">Номер шага</param>
        /// <returns>Успешность удаления</returns>
        public Result<bool> RemoveStep(int number)
        {
            if (number > _steps.Count)
            {
                return Result<bool>.Failure($"Шаблон не содержит шаг с таким номером");
            }

            _steps.RemoveAt(number-1);
            UpdateStepNumbers(number);

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Обмен номерами у шагов
        /// </summary>
        /// <param name="numberFirst">Номер первого шага</param>
        /// <param name="numberSecond">Номер второго шага</param>
        /// <returns>Успешность обмена</returns>
        public Result<bool> SwapSteps(int numberFirst, int numberSecond)
        {
            if (_steps.Count < numberFirst || _steps.Count < numberSecond) 
            {
                return Result<bool>.Failure($"Шаблон не содержит шаг с таким номером");
            }

            _steps[numberFirst - 1].UpdateNumber(numberSecond);
            _steps[numberSecond - 1].UpdateNumber(numberFirst);

            return Result<bool>.Success(true);
        }

        //Вынес в отдельный метод, т.к. может пригодиться в нескольких местах класса, а может и нет =)
        /// <summary>
        /// Проходит по писку от {number} и обновляет значение номера шага
        /// </summary>
        /// <param name="number">Номер шага, который был удален</param>
        private void UpdateStepNumbers(int number)
        {
            if (number <= _steps.Count)
            {
                for (int i = number - 1; i < _steps.Count; i++)
                {
                    _steps[i].UpdateNumber(i + 1);
                }
            }
        }
    }
}
