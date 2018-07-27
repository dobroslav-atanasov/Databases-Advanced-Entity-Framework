namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class BuyTicketCommand : ICommand
    {
        private readonly ICustomerService customerService;
        private readonly IBankAccountService bankAccountService;
        private readonly ITripService tripService;
        private readonly ITicketService ticketService;

        public BuyTicketCommand(ICustomerService customerService, IBankAccountService bankAccountService, ITripService tripService, ITicketService ticketService)
        {
            this.customerService = customerService;
            this.bankAccountService = bankAccountService;
            this.tripService = tripService;
            this.ticketService = ticketService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(4, arguments);
            int customerId = int.Parse(arguments[0]);
            int tripId = int.Parse(arguments[1]);
            decimal price = decimal.Parse(arguments[2]);
            int seat = int.Parse(arguments[3]);

            bool isCustomerExist = this.customerService.Exists(customerId);
            if (!isCustomerExist)
            {
                throw new ArgumentException(string.Format(AppConstants.CustomerDoesNotExist, customerId));
            }

            bool isTripExist = this.tripService.Exists(tripId);
            if (!isTripExist)
            {
                throw new ArgumentException(string.Format(AppConstants.TripDoesNotExist, tripId));
            }

            if (price <= 0)
            {
                throw new ArgumentException(AppConstants.InvalidBalance);
            }

            if (seat <= 0 && seat > 50)
            {
                throw new ArgumentException(AppConstants.InvalidSeat);
            }

            this.bankAccountService.Withdraw(customerId, price);
            this.ticketService.Add(customerId, tripId, price, seat);

            Customer customer = this.customerService.ById(customerId);

            return string.Format(AppConstants.AddTicket, customer.FirstName, customer.LastName, tripId, price, seat);
        }
    }
}