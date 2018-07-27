namespace BusTicketsSystem.Services.Contracts
{
    using Models;

    public interface ICustomerService
    {
        Customer ById(int id);

        bool Exists(int id);

        void Add(string firstName, string lastName, string genderString, int townId);
    }
}