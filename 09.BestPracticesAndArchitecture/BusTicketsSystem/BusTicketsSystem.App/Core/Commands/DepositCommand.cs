namespace BusTicketsSystem.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class DepositCommand : ICommand
    {
        private readonly IBankAccountService bankAccountService;
        private readonly ICustomerService customerService;

        public DepositCommand(IBankAccountService bankAccountService, ICustomerService customerService)
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

            bool isBankAccountExist = this.bankAccountService.ExistsByCustomerId(customerId);
            if (!isBankAccountExist)
            {
                throw new ArgumentException(AppConstants.BankAccountDoesNotExist);
            }

            this.bankAccountService.Deposit(customerId, amount);

            return AppConstants.DepositMoney;
        }
    }
}