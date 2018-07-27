namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class AddBusStationCommand : ICommand
    {
        private readonly IBusStationService busStationService;
        private readonly ITownService townService;

        public AddBusStationCommand(IBusStationService busStationService, ITownService townService)
        {
            this.busStationService = busStationService;
            this.townService = townService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            string name = arguments[0];
            string townName = arguments[1];

            bool isStationExist = this.busStationService.Exists(name);
            if (isStationExist)
            {
                throw new ArgumentException(string.Format(AppConstants.BusStationExists, name));
            }

            bool isTownExist = this.townService.Exists(townName);
            if (!isTownExist)
            {
                throw new ArgumentException(string.Format(AppConstants.TownDoesNotExist, townName));
            }

            Town town = this.townService.ByName(townName);
            this.busStationService.Add(name, town.Id);

            return string.Format(AppConstants.AddBusStation, name);
        }
    }
}