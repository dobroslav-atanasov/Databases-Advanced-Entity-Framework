namespace BusTicketsSystem.Services
{
    using Contracts;
    using Data;
    using Models;

    public class TicketService : ITicketService
    {
        private readonly BusTicketsSystemContext dbContext;

        public TicketService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(int customerId, int tripId, decimal price, int seat)
        {
            Ticket ticket = new Ticket()
            {
                CustomerId = customerId,
                TripId = tripId,
                Price = price,
                Seat = seat
            };

            this.dbContext.Tickets.Add(ticket);

            this.dbContext.SaveChanges();
        }
    }
}