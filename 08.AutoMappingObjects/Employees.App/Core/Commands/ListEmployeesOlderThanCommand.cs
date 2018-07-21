namespace Employees.App.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Contracts;
    using DtoModels;
    using Services.Contracts;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public ListEmployeesOlderThanCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new ArgumentException("Invalid age!");
            }

            int age = int.Parse(arguments[0]);
            IEnumerable<EmployeeManagerDto> employees = this.employeeService.ListAllEmployees(age);

            StringBuilder sb = new StringBuilder();
            foreach (EmployeeManagerDto dto in employees)
            {
                string manager = dto.ManagerLastName != null ? dto.ManagerLastName : "[no manager]";
                sb.AppendLine($"{dto.FirstName} {dto.LastName} - ${dto.Salary:F2} - Manager: {manager}");
            }

            return sb.ToString().Trim();
        }
    }
}