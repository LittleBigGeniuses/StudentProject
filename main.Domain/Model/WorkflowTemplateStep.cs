using main.domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main.domain.Model
{
    /// <summary>
    /// Шаблонный класс для объектов класса WorkFlowStep
    /// </summary>
    public class WorkflowTemplateStep : BaseEntity
    {
        /// <summary>
        /// Id объекта класса WorkflowTemplateStep
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Id объекта класса Employee
        /// </summary>
        public long employeуId { get; private set; }
        /// <summary>
        /// Id объекта класса Role
        /// </summary>
        public long roleId { get; private set; }
        /// <summary>
        /// Id объекта класса WorkflowTemplate
        /// </summary>
        public long workflowTemplateId { get; private set; }
        /// <summary>
        /// Дата создания объекта класса WorkflowTemplate
        /// </summary>
        new public DateTime DateCreate { get; set; }
        /// <summary>
        /// Дата обновления объекта класса WorkflowTemplate
        /// </summary>
        new public DateTime DateUpdate { get; set; }
        private string description { get; set; }
        private int numberOfStep { get; set; }

        private WorkflowTemplateStep(int NumberOfStep, string Description, DateTime dateTime)
        {
            numberOfStep = NumberOfStep;
            description = Description;
            DateCreate = dateTime;
            DateUpdate = dateTime;
        }
        /// <summary>
        /// метод для создание WorkflowTemplateStep объекта
        /// </summary>
        /// <param name="NumberOfStep"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public static Result<WorkflowTemplateStep> Create(int NumberOfStep, string Description)
        {
            if (NumberOfStep < 0)
                return Result<WorkflowTemplateStep>.Failure("Номер шаблонного этапа некорректен");
            if (String.IsNullOrEmpty(Description))
                return Result<WorkflowTemplateStep>.Failure("Описание шаблонного этапа не может быть пустым");
            return Result<WorkflowTemplateStep>.Success(new WorkflowTemplateStep(NumberOfStep, Description, DateTime.Now));
        }

    }
}
