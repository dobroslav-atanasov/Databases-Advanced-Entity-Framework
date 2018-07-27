namespace BusTicketsSystem.Services.Contracts
{
    using Models;

    public interface IBusCompanyService
    {
        BusCompany ById(int id);

        BusCompany ByName(string name);

        bool Exists(int id);

        bool Exists(string name);

        void Add(string name);
    }
}