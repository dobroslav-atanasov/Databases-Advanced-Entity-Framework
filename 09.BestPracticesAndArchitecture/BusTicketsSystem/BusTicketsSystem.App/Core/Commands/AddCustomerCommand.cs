namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Models.Enums;
    using Services.Contracts;
    using Utilities;

    public class AddCustomerCommand : ICommand
    {
        private readonly ICustomerService customerService;
        private readonly ITownService townService;

        public AddCustomerCommand(ICustomerService customerService, ITownService townService)
        {
            this.customerService = customerService;
            this.townService = townService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(4, arguments);
            string firstName = arguments[0];
            string lastName = arguments[1];
            string genderString = arguments[2];
            string townName = arguments[3];

            bool isValidGender = Enum.TryParse<Gender>(genderString, out Gender gender);
            if (!isValidGender)
            {
                throw new ArgumentException(AppConstants.NotValidGender);
            }

            bool isTownExist = this.townService.Exists(townName);
            if (!isTownExist)
            {
                throw new ArgumentException(string.Format(AppConstants.TownDoesNotExist, townName));
            }

            Town town = this.townService.ByName(townName);
            this.customerService.Add(firstName, lastName, genderString, town.Id);

            return string.Format(AppConstants.AddCustomer, firstName, lastName);
        }
    }
}