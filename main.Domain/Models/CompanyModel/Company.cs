using main.domain.Common;
using main.domain.Models.CondidateModel;
using main.domain.Models.EmployeeModel;
using main.domain.Models.WorkflowModel;
using main.domain.Models.WorkflowTemplateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace main.domain.Models.CompanyModel
{
    /// <summary>
    /// Класс компании
    /// </summary>
    public class Company : BaseEntity
    {
        /// <summary>
        /// Список шаблонов рабочего процесса в компании
        /// </summary>
        private readonly List<WorkflowTemplate> _workflowTemplates = new();

        /// <summary>
        /// Список сотрудников, работающих в компании
        /// </summary>
        private readonly List<Employee> _employees = new();

        /// <summary>
        /// Список должностей в компании
        /// </summary>
        private readonly List<Role> _roles = new();

        /// <summary>
        /// Список рабочих процессов в компании
        /// </summary>
        private readonly List<Workflow> _workflows = new();


        private Company(string name)
        {
            Name = name;
            DateCreate = DateTime.Now;
            DateUpdate = DateTime.Now;
        }
        /// <summary>
        /// Название компании
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Добавить сотрудника
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="roleId">Идентификатор должности</param>
        /// <returns></returns>
        public Result<Employee> AddEmployee(string name, long roleId)
        {
            var createEmployee = Employee.Create(name, Id,  roleId);

            if (createEmployee.IsFailure)
            {
                return Result<Employee>.Failure($"Добавление элемента в список провалиловсь: {createEmployee.FailureMessage}");
            }

            var employee = createEmployee.Data;

            _employees.Add(employee);

            return Result<Employee>.Success(employee);
        }

        /// <summary>
        /// Добавить рабочий процесс
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="description">Описание</param>
        /// <returns></returns>
        public Result<WorkflowTemplate> AddTemplate(string name, string description)
        {
            var createTemplate = WorkflowTemplate.Create(name, description, Id);

            if (createTemplate.IsFailure)
            {
                return Result<WorkflowTemplate>.Failure($"Добавление элемента в список провалиловсь: {createTemplate.FailureMessage}");
            }

            var template = createTemplate.Data;

            _workflowTemplates.Add(template);

            return Result<WorkflowTemplate>.Success(template);
        }

        /// <summary>
        /// Добавить должность
        /// </summary>
        /// <param name="name">Название</param>
        /// <returns></returns>
        public Result<Role> AddRole(string name)
        {
            var createRole = Role.Create(name, Id);

            if (createRole.IsFailure)
            {
                return Result<Role>.Failure($"Добавление элемента в список провалиловсь: {createRole.FailureMessage}");
            }

            var role = createRole.Data;

            _roles.Add(role);

            return Result<Role>.Success(role);
        }

        /// <summary>
        /// Добавить рабочий процесс
        /// </summary>
        /// <param name="authorId">Идентификатор сотрудника, создавшего рабочий процесс</param>
        /// <param name="candidateId">Идентификатор кандидата</param>
        /// <param name="template">Сущность шаблона этого рабочего процесса</param>
        /// <returns></returns>
        public Result<Workflow> AddWorkflow(long authorId, long candidateId, WorkflowTemplate template)
        {
            var createWorkflow = Workflow.Create(authorId, candidateId, template);

            if (createWorkflow.IsFailure)
            {
                return Result<Workflow>.Failure($"Добавление элемента в список провалиловсь: {createWorkflow.FailureMessage}");
            }

            var workflow = createWorkflow.Data;

            _workflows.Add(workflow);

            return Result<Workflow>.Success(workflow);
        }
        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <param name="employeeId">Ижентификатор сотрудника</param>
        /// <returns></returns>
        public Result<bool> RemoveEmployee(long employeeId)
        {
            foreach (var employee in _employees) 
            {
                if (employee.Id == employeeId)
                {
                    _employees.Remove(employee);
                    return Result<bool>.Success(true);
                }
            }

            return Result<bool>.Failure($"Сотрудника с идентификационным номером {employeeId} не существует"); ;
        }

        /// <summary>
        /// Удалить шаблон рабочего процесса
        /// </summary>
        /// <param name="templateId">Идентификатор шаблона рабочего процесса</param>
        /// <returns></returns>
        public Result<bool> RemoveTemplate(long templateId)
        {
            foreach (var template in _workflowTemplates)
            {
                if (template.Id == templateId)
                {
                    _workflowTemplates.Remove(template);
                    return Result<bool>.Success(true);
                }
            }

            return Result<bool>.Failure($"Шаблона с идентификационным номером {templateId} не существует");
        }

        /// <summary>
        /// Удалить должность
        /// </summary>
        /// <param name="roleId">Идентификатор должности</param>
        /// <returns></returns>
        public Result<bool> RemoveRole(long roleId)
        {
            foreach (var role in _roles)
            {
                if (role.Id == roleId)
                {
                    _roles.Remove(role);
                    return Result<bool>.Success(true);
                }
            }

            return Result<bool>.Failure($"Должности с идентификационным номером {roleId} не существует");
        }

        /// <summary>
        /// Удалить рабочий процесс
        /// </summary>
        /// <param name="workflowId">Идентификатор рабочего процесса</param>
        /// <returns></returns>
        public Result<bool> RemoveWorkflow(long workflowId)
        {
            foreach (var workflow in _workflows)
            {
                if (workflow.Id == workflowId)
                {
                    _workflows.Remove(workflow);
                    return Result<bool>.Success(true);
                }
            }

            return Result<bool>.Failure($"Рабочего процесса с идентификационным номером {workflowId} не существует");
        }
    }
}
