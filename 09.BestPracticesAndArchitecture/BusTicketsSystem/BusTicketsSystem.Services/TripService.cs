namespace BusTicketsSystem.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;

    public class TripService : ITripService
    {
        private readonly BusTicketsSystemContext dbContext;

        public TripService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Trip ById(int id)
        {
            Trip trip = this.dbContext
                .Trips
                .FirstOrDefault(t => t.Id == id);

            return trip;
        }

        public bool Exists(int id)
        {
            bool isExist = this.dbContext
                .Trips
                .Any(t => t.Id == id);

            return isExist;
        }

        public void Add(int originId, int destionationId, int busCompanyId, string statusString, DateTime departureTime,
            DateTime arrivalTime)
        {
            Status status = Enum.Parse<Status>(statusString);

            Trip trip = new Trip()
            {
                OriginBusStationId = originId,
                DestinationBusStationId = destionationId,
                BusCompanyId = busCompanyId,
                Status = status,
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime
            };

            this.dbContext.Trips.Add(trip);

            this.dbContext.SaveChanges();
        }

        public void ChangeTripStatus(int tripId, string newStatusString)
        {
            Status newStatus = Enum.Parse<Status>(newStatusString);

            Trip trip = this.ById(tripId);

            trip.Status = newStatus;

            this.dbContext.SaveChanges();
        }
    }
}