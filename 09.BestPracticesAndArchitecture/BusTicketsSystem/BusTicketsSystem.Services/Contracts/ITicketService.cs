namespace BusTicketsSystem.Services.Contracts
{
    public interface ITicketService
    {
        void Add(int customerId, int tripId, decimal price, int seat);
    }
}