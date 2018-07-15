namespace BillsPaymentSystem.App.Core.Commands
{
    using System.Text;
    using Contracts;

    public class HelpCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(new string('-', 85));
            sb.AppendLine($"|Command Name{new string(' ', 16)}|Description{new string(' ', 43)}|");
            sb.AppendLine(new string('-', 85));
            sb.AppendLine($"|DepositBankAccount{new string(' ', 10)}|Adds money to bank account of given user.{new string(' ', 13)}|");
            sb.AppendLine($"|DepositCreditCard{new string(' ', 11)}|Adds money to credit card of given user.{new string(' ', 14)}|");
            sb.AppendLine($"|Exit{new string(' ', 24)}|Exit from the program.{new string(' ', 32)}|");
            sb.AppendLine($"|Help{new string(' ', 24)}|Shows all posible commands.{new string(' ', 27)}|");
            sb.AppendLine($"|ListAllUsers{new string(' ', 16)}|Shows all users (first name, last name and emails).{new string(' ', 3)}|");
            sb.AppendLine($"|PayBills{new string(' ', 20)}|Pay bills by user.{new string(' ', 36)}|");
            sb.AppendLine($"|SearchUserById{new string(' ', 14)}|Search user by id.{new string(' ', 36)}|");
            sb.AppendLine($"|UserDetails{new string(' ', 17)}|Shows user bank accounts and credit cards.{new string(' ', 12)}|");
            sb.AppendLine(new string('-', 85));

            return sb.ToString().Trim();
        }
    }
}