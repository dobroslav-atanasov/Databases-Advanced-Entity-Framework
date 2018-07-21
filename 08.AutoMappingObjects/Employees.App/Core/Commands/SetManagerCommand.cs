namespace Employees.App.Core.Commands
{
    using System;
    using Contracts;
    using Services.Contracts;

    public class SetManagerCommand : ICommand
    {
        private readonly IManagerService managerService;

        public SetManagerCommand(IManagerService managerService)
        {
            this.managerService = managerService;
        }

        public string Execute(string[] arguments)
        {
            if (arguments.Length != 2)
            {
                throw new ArgumentException("Invalid employee id or manager id!");
            }

            int employeeId = int.Parse(arguments[0]);
            int managerId = int.Parse(arguments[1]);

            this.managerService.SetManager(employeeId, managerId);
            return $"Employee with id {employeeId} set manager with id {managerId}";
        }
    }
}