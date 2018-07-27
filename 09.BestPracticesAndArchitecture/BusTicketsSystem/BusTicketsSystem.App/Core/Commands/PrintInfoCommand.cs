namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Contracts;
    using Models;
    using Models.Enums;
    using Services.Contracts;
    using Utilities;

    public class PrintInfoCommand : ICommand
    {
        private readonly IBusStationService busStationService;

        public PrintInfoCommand(IBusStationService busStationService)
        {
            this.busStationService = busStationService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            int stationId = int.Parse(arguments[0]);

            bool isValidStation = this.busStationService.Exists(stationId);
            if (!isValidStation)
            {
                throw new ArgumentException(AppConstants.BusStationDoesNotExist);
            }

            BusStation busStation = this.busStationService.ById(stationId);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{busStation.Name}, {busStation.Town.Name}");
            sb.AppendLine($"Arrivals: ");
            foreach (Trip trip in busStation.DestinationTrips)
            {
                sb.AppendLine($"From {trip.OriginBusStation.Name} | Arrive at: {trip.ArrivalTime} | Status: {trip.Status}");
            }
            sb.AppendLine($"Departures: ");
            foreach (Trip trip in busStation.OriginTrips)
            {
                sb.AppendLine($"To {trip.DestinationBusStation.Name} | Depart at: {trip.DepartureTime} | Status: {trip.Status}");
            }

            return sb.ToString().Trim();
        }
    }
}