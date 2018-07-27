namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Services.Contracts;
    using Utilities;

    public class AddBusCompanyCommand : ICommand
    {
        private readonly IBusCompanyService busCompanyService;

        public AddBusCompanyCommand(IBusCompanyService busCompanyService)
        {
            this.busCompanyService = busCompanyService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            string name = arguments[0];

            bool isExist = this.busCompanyService.Exists(name);
            if (isExist)
            {
                throw new ArgumentException(string.Format(AppConstants.BusCompanyExists, name));
            }

            this.busCompanyService.Add(name);

            return string.Format(AppConstants.AddBusCompany, name);
        }
    }
}