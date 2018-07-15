namespace BillsPaymentSystem.App.Core.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using IO.Contracts;
    using Models;
    using Services.Contracts;

    public class PayBillsCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IBankAccountService bankAccountService;
        private readonly ICreditCardService creditCardService;
        private readonly IReader reader;
        private readonly IWriter writer;

        public PayBillsCommand(IUserService userService, IBankAccountService bankAccountService, ICreditCardService creditCardService, IReader reader, IWriter writer)
        {
            this.userService = userService;
            this.bankAccountService = bankAccountService;
            this.creditCardService = creditCardService;
            this.reader = reader;
            this.writer = writer;
        }

        public string Execute(string[] arguments)
        {
            this.writer.Write("User ID: ");
            int userId = int.Parse(this.reader.ReadLine());
            this.writer.Write("Amount: ");
            decimal amount = decimal.Parse(this.reader.ReadLine());

            User user = this.userService.ById(userId);
            List<BankAccount> bankAccounts = user
                .PaymentMethods
                .Select(pm => pm.BankAccount)
                .ToList();

            List<CreditCard> creditCards = user
                .PaymentMethods
                .Select(pm => pm.CreditCard)
                .ToList();

            foreach (BankAccount bankAccount in bankAccounts)
            {
                if (bankAccount.Balance > amount)
                {
                    this.bankAccountService.Withdraw(bankAccount, amount);
                    return "Transaction was successfully!";
                }
            }

            foreach (CreditCard creditCard in creditCards)
            {
                if (creditCard.MoneyOwed > amount)
                {
                    this.creditCardService.Withdraw(creditCard, amount);
                    return "Transaction was successfully!";
                }
            }

            return "Insufficient funds!";
        }
    }
}