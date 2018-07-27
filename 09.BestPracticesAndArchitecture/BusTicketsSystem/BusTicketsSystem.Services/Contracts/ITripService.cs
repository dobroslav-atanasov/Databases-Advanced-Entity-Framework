namespace BusTicketsSystem.Services.Contracts
{
    using System;
    using Models;

    public interface ITripService
    {
        Trip ById(int id);

        bool Exists(int id);

        void Add(int originId, int destionationId, int busCompanyId, string statusString, DateTime departureTime, DateTime arrivalTime);

        void ChangeTripStatus(int tripId, string newStatusString);
    }
}