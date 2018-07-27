namespace BusTicketsSystem.Services.Contracts
{
    using Models;

    public interface IBusStationService
    {
        BusStation ById(int id);

        BusStation ByName(string name);

        bool Exists(int id);

        bool Exists(string name);

        void Add(string name, int townId);
    }
}