namespace Employees.App.Core.Commands
{
    using System;
    using Contracts;
    using DtoModels;
    using Services.Contracts;

    public class EmployeeInfoCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public EmployeeInfoCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new ArgumentException("Invalid id!");
            }

            int id = int.Parse(arguments[0]);
            EmployeeDto employeeDto = this.employeeService.EmployeeInfo(id);

            string result = $"{employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:F2}";
            return result;
        }
    }
}