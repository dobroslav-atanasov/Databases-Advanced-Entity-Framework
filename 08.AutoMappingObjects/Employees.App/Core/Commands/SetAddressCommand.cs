namespace Employees.App.Core.Commands
{
    using System;
    using Contracts;
    using Services.Contracts;

    public class SetAddressCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public SetAddressCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        
        public string Execute(string[] arguments)
        {
            if (arguments.Length != 2)
            {
                throw new ArgumentException("Invalid id or address!");
            }

            int id = int.Parse(arguments[0]);
            string address = arguments[1];

            this.employeeService.SetAddress(id, address);
            return $"Address was set on employee with id {id}";
        }
    }
}