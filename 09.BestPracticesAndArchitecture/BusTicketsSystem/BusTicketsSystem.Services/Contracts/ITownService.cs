namespace BusTicketsSystem.Services.Contracts
{
    using Models;

    public interface ITownService
    {
        Town ById(int id);

        Town ByName(string name);

        bool Exists(int id);

        bool Exists(string name);

        void Add(string townName, string country);
    }
}