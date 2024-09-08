using main.domain.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Model
{
    /// <summary>
    /// шаблонный класс для объектов класса Workflow
    /// </summary>
    public class WorkflowTemplate : BaseEntity
    {
        private readonly List<WorkflowTemplateStep> workflowTemplateSteps = new List<WorkflowTemplateStep>();
        /// <summary>
        /// Id объекта класса WoekflowTemplate
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Id объекта класса Company
        /// </summary>
        public long companyId { get; private set; }
        /// <summary>
        /// Дата создания объекта класса WorkflowTemplate
        /// </summary>
        new public DateTime DateCreate { get; set; }
        /// <summary>
        /// Дата обновления объекта класса WorkflowTemplate
        /// </summary>
        new public DateTime DateUpdate { get; set; }
        private string name { get; set; }
        private string description { get; set; }
        private WorkflowTemplate(string Name, string Description, DateTime dateTime)
        {
            name = Name;
            description = Description;
            DateCreate = dateTime;
            DateUpdate = dateTime;
        }
        /// <summary>
        /// Метод для создания WorkflowTemplate объекта
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public static Result<WorkflowTemplate> Create(string Name, string Description)
        {
            if (String.IsNullOrEmpty(Name))
            {
                return Result<WorkflowTemplate>.Failure("Название рабочего процесса не может быть пустым");
            }
            if (String.IsNullOrEmpty(Description))
            {
                return Result<WorkflowTemplate>.Failure("Описание рабочего процесса не может быть пустым");
            }
            return Result<WorkflowTemplate>.Success(new WorkflowTemplate(Name, Description, DateTime.Now));
        }
        /// <summary>
        /// Метод для изменения информации об WorkflowTemplate объекте
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        public Result<WorkflowTemplate> UpdateInfo(string Name, string Description)
        {
            if (String.IsNullOrEmpty(Name))
            {
                return Result<WorkflowTemplate>.Failure("Название рабочего процесса не может быть пустым");
            }
            if (String.IsNullOrEmpty(Description))
            {
                return Result<WorkflowTemplate>.Failure("Описание рабочего процесса не может быть пустым");
            }
            name = Name;
            description = Description;
            DateUpdate = DateTime.Now;
            return Result<WorkflowTemplate>.Success(this);
        }
        /// <summary>
        /// Добавляет новый шаг в список шагов собеседования workflowTemplateSteps
        /// </summary>
        /// <param name="step"></param>
        public void AddStep(WorkflowTemplateStep step)
        {
            DateUpdate = DateTime.Now;

            workflowTemplateSteps.Add(step);
        }
        /// <summary>
        /// Изменяет шаг указанного индекса на указанный в параметрах шаг в списке шагов собеседования workflowTemplateSteps
        /// </summary>
        /// <param name="number"></param>
        /// <param name="step"></param>
        public void UpdateStep(int number, WorkflowTemplateStep step)
        {
            DateUpdate = DateTime.Now;

            workflowTemplateSteps[number] = step;
        }
        /// <summary>
        /// Вставляет указанный в параметрах шаг на место указанного индекса в списке шагов собеседования workflowTemplateSteps
        /// </summary>
        /// <param name="number"></param>
        /// <param name="step"></param>
        public void InsertStep(int number, WorkflowTemplateStep step)
        {
            DateUpdate = DateTime.Now;

            workflowTemplateSteps.Insert(number, step);
        }
        /// <summary>
        /// Удаляет указанный индексом шаг в списке шагов собеседования workflowTemplateSteps
        /// </summary>
        /// <param name="number"></param>
        public void RemoveStep(int number)
        {
            DateUpdate = DateTime.Now;

            workflowTemplateSteps.RemoveAt(number);
        }
        

    }
}
