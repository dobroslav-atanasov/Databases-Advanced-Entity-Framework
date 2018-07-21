namespace Employees.App.Core.Commands
{
    using System;
    using System.Text;
    using Contracts;
    using DtoModels;
    using Services.Contracts;

    public class ManagerInfoCommand : ICommand
    {
        private readonly IManagerService managerService;

        public ManagerInfoCommand(IManagerService managerService)
        {
            this.managerService = managerService;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.Length != 1)
            {
                throw new ArgumentException("Invalid id!");
            }

            int id = int.Parse(arguments[0]);

            ManagerDto managerDto = this.managerService.ManagerInfo(id);

            if (managerDto.EmployeesCount == 0)
            {
                return $"Manager {managerDto.FirstName} {managerDto.LastName} does not have any employees!";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.EmployeesCount}");
            foreach (EmployeeDto empDto in managerDto.Employees)
            {
                sb.AppendLine($"    - {empDto.FirstName} {empDto.LastName} - ${empDto.Salary:F2}");
            }

            return sb.ToString().Trim();
        }
    }
}