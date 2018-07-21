namespace Employees.App.Core.Commands
{
    using System;
    using Contracts;
    using DtoModels;
    using Services.Contracts;

    public class AddEmployeeCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public AddEmployeeCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.Length != 3)
            {
                throw new ArgumentException("Invalid first name or last name or salary!");
            }

            string firstName = arguments[0];
            string lastName = arguments[1];
            decimal salary = decimal.Parse(arguments[2]);

            EmployeeDto employeeDto = new EmployeeDto(firstName, lastName, salary);
            this.employeeService.AddEmployee(employeeDto);

            return $"Employee {firstName} {lastName} was added successfully!";
        }
    }
}