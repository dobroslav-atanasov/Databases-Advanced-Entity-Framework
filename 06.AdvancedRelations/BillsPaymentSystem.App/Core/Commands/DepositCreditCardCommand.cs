namespace BillsPaymentSystem.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using IO.Contracts;
    using Models;
    using Services.Contracts;

    public class DepositCreditCardCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly ICreditCardService creditCardService;
        private readonly IReader reader;
        private readonly IWriter writer;

        public DepositCreditCardCommand(IUserService userService, ICreditCardService creditCardService, IReader reader, IWriter writer)
        {
            this.userService = userService;
            this.creditCardService = creditCardService;
            this.reader = reader;
            this.writer = writer;
        }

        public string Execute(string[] arguments)
        {
            this.writer.Write("User ID: ");
            int userId = int.Parse(this.reader.ReadLine());
            this.writer.Write("Credit Card ID: ");
            int creditCardId = int.Parse(this.reader.ReadLine());
            this.writer.Write("Amount: ");
            decimal amount = decimal.Parse(this.reader.ReadLine());

            User user = this.userService.ById(userId);
            CreditCard creditCard = user
                .PaymentMethods
                .Select(pm => pm.CreditCard)
                .SingleOrDefault(ba => ba.CreditCardId == creditCardId);

            if (creditCard == null)
            {
                throw new ArgumentException($"Credit Card with id {creditCardId} does not exist");
            }

            this.creditCardService.Deposit(creditCard, amount);

            return "Transanction was succesfully!";
        }
    }
}