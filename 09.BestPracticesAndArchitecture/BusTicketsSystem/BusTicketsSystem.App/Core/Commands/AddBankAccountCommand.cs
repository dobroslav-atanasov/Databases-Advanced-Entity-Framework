namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class AddBankAccountCommand : ICommand
    {
        private readonly IBankAccountService bankAccountService;
        private readonly ICustomerService customerService;

        public AddBankAccountCommand(IBankAccountService bankAccountService, ICustomerService customerService)
        {
            this.bankAccountService = bankAccountService;
            this.customerService = customerService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            int customerId = int.Parse(arguments[0]);
            decimal amount = decimal.Parse(arguments[1]);

            bool isCustomerExist = this.customerService.Exists(customerId);
            if (!isCustomerExist)
            {
                throw new ArgumentException(string.Format(AppConstants.CustomerDoesNotExist, customerId));
            }

            if (amount <= 0)
            {
                throw new ArgumentException(AppConstants.InvalidBalance);
            }

            this.bankAccountService.Add(customerId, amount);

            return AppConstants.AddBankAccount;
        }
    }
}