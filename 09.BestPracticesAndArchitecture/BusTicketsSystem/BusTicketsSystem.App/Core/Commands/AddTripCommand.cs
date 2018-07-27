namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using System.Globalization;
    using Contracts;
    using Models;
    using Models.Enums;
    using Services.Contracts;
    using Utilities;

    public class AddTripCommand : ICommand
    {
        private readonly ITripService tripService;
        private readonly IBusStationService busStationService;
        private readonly IBusCompanyService busCompanyService;

        public AddTripCommand(ITripService tripService, IBusStationService busStationService, IBusCompanyService busCompanyService)
        {
            this.tripService = tripService;
            this.busStationService = busStationService;
            this.busCompanyService = busCompanyService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(8, arguments);
            string originBusStation = arguments[0];
            string destinationBusStation = arguments[1];
            string busCompanyName = arguments[2];
            string statusString = arguments[3];
            string departureDate = arguments[4];
            string departureTime = arguments[5];
            string arrivalDate = arguments[6];
            string arrivalTime = arguments[7];

            bool isOriginExist = this.busStationService.Exists(originBusStation);
            bool isDestinationExist = this.busStationService.Exists(destinationBusStation);
            if (!isOriginExist || !isDestinationExist)
            {
                throw new ArgumentException(AppConstants.InvalidTowns);
            }

            bool isCompanyExist = this.busCompanyService.Exists(busCompanyName);
            if (!isCompanyExist)
            {
                throw new ArgumentException(string.Format(AppConstants.BusCompanyDoesNotExist, busCompanyName));
            }

            bool isValidStatus = Enum.TryParse<Status>(statusString, out Status status);
            if (!isValidStatus)
            {
                throw new ArgumentException(AppConstants.NotValidStatus);
            }

            string departureDateTime = $"{departureDate} {departureTime}";
            string arrivalDateTime = $"{arrivalDate} {arrivalTime}";

            DateTime departure;
            DateTime arrival;
            bool isValidDeparture = DateTime.TryParseExact(departureDateTime, AppConstants.DateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out departure);
            bool isValidArrival = DateTime.TryParseExact(arrivalDateTime, AppConstants.DateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out arrival);

            if (!isValidDeparture || !isValidArrival)
            {
                throw new ArgumentException(AppConstants.InvalidDateTimeFormat);
            }

            BusStation origin = this.busStationService.ByName(originBusStation);
            BusStation destiation = this.busStationService.ByName(destinationBusStation);
            BusCompany busCompany = this.busCompanyService.ByName(busCompanyName);

            this.tripService.Add(origin.Id, destiation.Id, busCompany.Id, statusString, departure, arrival);
            return AppConstants.AddTrip;
        }
    }
}