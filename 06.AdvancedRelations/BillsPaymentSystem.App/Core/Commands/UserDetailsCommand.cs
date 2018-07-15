namespace BillsPaymentSystem.App.Core.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Contracts;
    using IO.Contracts;
    using Models;
    using Services.Contracts;

    public class UserDetailsCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IReader reader;
        private readonly IWriter writer;

        public UserDetailsCommand(IUserService userService, IReader reader, IWriter writer)
        {
            this.userService = userService;
            this.reader = reader;
            this.writer = writer;
        }

        public string Execute(string[] arguments)
        {
            this.writer.Write("User ID: ");
            int userId = int.Parse(this.reader.ReadLine());
            User user = this.userService.ById(userId);

            List<BankAccount> bankAccounts = user
                .PaymentMethods
                .Select(pm => pm.BankAccount)
                .ToList();

            List<CreditCard> creditCards = user
                .PaymentMethods
                .Select(pm => pm.CreditCard)
                .ToList();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"User {user.FirstName} {user.LastName}");
            if (bankAccounts.Count >= 1)
            {
                sb.AppendLine("Bank Accounts:");
                foreach (BankAccount bankAccount in bankAccounts)
                {
                    sb.AppendLine(bankAccount.ToString());
                }
            }

            if (creditCards.Count >= 1)
            {
                sb.AppendLine("Credit Cards:");
                foreach (CreditCard creditCard in creditCards)
                {
                    sb.AppendLine(creditCard.ToString());
                }
            }

            return sb.ToString().Trim();
        }
    }
}