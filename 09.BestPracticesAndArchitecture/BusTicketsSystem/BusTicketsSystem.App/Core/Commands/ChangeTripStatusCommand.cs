namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Models.Enums;
    using Services.Contracts;
    using Utilities;

    public class ChangeTripStatusCommand : ICommand
    {
        private readonly ITripService tripService;

        public ChangeTripStatusCommand(ITripService tripService)
        {
            this.tripService = tripService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            int tripId = int.Parse(arguments[0]);
            string newStatusString = arguments[1];

            bool isTripExist = this.tripService.Exists(tripId);
            if (!isTripExist)
            {
                throw new ArgumentException(string.Format(AppConstants.TripDoesNotExist, tripId));
            }

            bool isValidStatus = Enum.TryParse<Status>(newStatusString, out Status status);
            if (!isValidStatus)
            {
                throw new ArgumentException(AppConstants.NotValidStatus);
            }

            Trip trip = this.tripService.ById(tripId);
            string oldStatus = trip.Status.ToString();
            this.tripService.ChangeTripStatus(tripId, newStatusString);

            return string.Format(AppConstants.ChangeTripStatus, trip.OriginBusStation.Town.Name, trip.DestinationBusStation.Town.Name, trip.DepartureTime, oldStatus, newStatusString);
        }
    }
}