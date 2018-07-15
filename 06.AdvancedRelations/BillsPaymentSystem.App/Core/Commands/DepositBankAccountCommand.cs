namespace BillsPaymentSystem.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using IO.Contracts;
    using Models;
    using Services.Contracts;

    public class DepositBankAccountCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IBankAccountService bankAccountService;
        private readonly IReader reader;
        private readonly IWriter writer;

        public DepositBankAccountCommand(IUserService userService, IBankAccountService bankAccountService, IReader reader, IWriter writer)
        {
            this.userService = userService;
            this.bankAccountService = bankAccountService;
            this.reader = reader;
            this.writer = writer;
        }

        public string Execute(string[] arguments)
        {
            this.writer.Write("User ID: ");
            int userId = int.Parse(this.reader.ReadLine());
            this.writer.Write("Bank Account ID: ");
            int bankAccountId = int.Parse(this.reader.ReadLine());
            this.writer.Write("Amount: ");
            decimal amount = decimal.Parse(this.reader.ReadLine());

            User user = this.userService.ById(userId);
            BankAccount bankAccount = user
                .PaymentMethods
                .Select(pm => pm.BankAccount)
                .SingleOrDefault(ba => ba.BankAccountId == bankAccountId);

            if (bankAccount == null)
            {
                throw new ArgumentException($"Bank Account with id {bankAccountId} does not exist");
            }

            this.bankAccountService.Deposit(bankAccount, amount);

            return "Transanction was succesfully!";
        }
    }
}