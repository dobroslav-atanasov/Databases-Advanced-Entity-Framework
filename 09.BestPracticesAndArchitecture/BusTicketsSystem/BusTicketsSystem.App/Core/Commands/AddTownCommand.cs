namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Services.Contracts;
    using Utilities;

    public class AddTownCommand : ICommand
    {
        private readonly ITownService townService;

        public AddTownCommand(ITownService townService)
        {
            this.townService = townService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            string townName = arguments[0];
            string country = arguments[1];

            bool isExist = this.townService.Exists(townName);
            if (isExist)
            {
                throw new ArgumentException(string.Format(AppConstants.TownExists, townName));
            }

            this.townService.Add(townName, country);

            return string.Format(AppConstants.AddTown, townName);
        }
    }
}