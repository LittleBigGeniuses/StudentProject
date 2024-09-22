﻿using main.domain.Common;

namespace main.domain.WorkflowTemplate
{
    /// <summary>
    /// Шаблон Workfloy
    /// </summary>
    public class WorkflowTemplate
    {
        /// <summary>
        /// Константное значение минимальной длины наименования шаблона
        /// </summary>
        public const int MinLengthName = 5;


        private WorkflowTemplate(
            Guid id,
            string name, 
            string description, 
            List<WorkflowStepTemplate> stepTemplates, 
            Guid companyId, DateTime dateCreate, 
            DateTime dateUpdate)
        {
            Id = id;
            Name = name;
            Description = description;
            _steps = stepTemplates;
            CompanyId = companyId;
            DateCreate = dateCreate;
            DateUpdate = dateUpdate;

        }

        /// <summary>
        /// Метод создание нового шаблона
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <param name="companyId">Идентификатор компании</param>
        /// <returns>Сущность с результатом создания</returns>
        public static Result<WorkflowTemplate> Create(string name, string description, Guid companyId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result<WorkflowTemplate>.Failure("Наименование шаблона не может быть пустым");
            }

            if (name.Trim().Length < MinLengthName)
            {
                return Result<WorkflowTemplate>.Failure($"Длина наименование шаблона не может быть меньше {MinLengthName}");
            }

            if (companyId == Guid.Empty)
            {
                return Result<WorkflowTemplate>.Failure($"{companyId} - некорректный идентификатор компании");
            }

            var workflowTemplate = new WorkflowTemplate(
                Guid.NewGuid(), 
                name.Trim(), 
                description, 
                [] ,
                companyId, 
                DateTime.UtcNow, 
                DateTime.UtcNow);

            return Result<WorkflowTemplate>.Success(workflowTemplate);
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
        /// Идентификатор компании, которой принадлежит шаблон
        /// </summary>
        public Guid CompanyId { get; }

        /// <summary>
        /// Безопасный список, для чтения из вне
        /// </summary>
        public IReadOnlyCollection<WorkflowStepTemplate> Steps => _steps;

        /// <summary>
        /// Защищенный список шагов
        /// </summary>
        private List<WorkflowStepTemplate> _steps;


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
        /// Добавление нового шага
        /// </summary>
        /// <param name="description">Описание</param>
        /// <param name="employerId">Идентификатор сотрудника, исполняемого шаг</param>
        /// <param name="roleId">Идентификатор должности, исполняемой шаг</param>
        /// <returns>Результат добавления шага</returns>
        public Result<bool> AddStep(string description, Guid? employerId, Guid? roleId)
        {
            var createStep = WorkflowStepTemplate.Create(_steps.Count + 1, description, employerId, roleId);

            if (createStep.IsFailure)
            {
                return Result<bool>.Failure($"Добавление элемента в список провалиловсь: {createStep.FailureMessage}");
            }

            var step = createStep.Data;

            _steps.Add(step!);

            DateUpdate = DateTime.UtcNow;

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

            _steps.RemoveAt(number - 1);
            UpdateStepNumbers(number);

            DateUpdate = DateTime.UtcNow;

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

            DateUpdate = DateTime.UtcNow;

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