namespace Employees.App.Core.Commands
{
    using System;
    using System.Text;
    using Contracts;
    using DtoModels;
    using Services.Contracts;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public EmployeePersonalInfoCommand(IEmployeeService employeeService)
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

            EmployeePersonalDto epDto = this.employeeService.EmployeePersonalInfo(id);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ID: {epDto.Id} - {epDto.FirstName} {epDto.LastName} - ${epDto.Salary:F2}");
            sb.AppendLine($"Birthday: {epDto.Birthday.Value.ToString("dd-MM-yyyy")}");
            sb.AppendLine($"Address: {epDto.Address}");

            return sb.ToString().Trim();
        }
    }
}