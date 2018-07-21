namespace Employees.App.Core.Commands
{
    using System;
    using Contracts;
    using Services.Contracts;

    public class SetBirthdayCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public SetBirthdayCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.Length != 2)
            {
                throw new ArgumentException("Invalid id or date!");
            }

            int id = int.Parse(arguments[0]);
            DateTime date = DateTime.ParseExact(arguments[1], "dd-MM-yyyy", null);

            this.employeeService.SetBirthday(id, date);
            return $"Birthday was set on employee with id {id}!";
        }
    }
}