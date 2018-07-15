namespace BillsPaymentSystem.App.Core.Commands
{
    using System.Text;
    using Contracts;
    using Models;
    using Services.Contracts;

    public class ListAllUsersCommand : ICommand
    {
        private readonly IUserService userService;

        public ListAllUsersCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            User[] users = this.userService.ListAllUsers();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(new string('-', 90));
            sb.AppendLine($"|First name{new string(' ', 18)}|Last name{new string(' ', 20)}|Email{new string(' ', 24)}|");
            sb.AppendLine(new string('-', 90));
            foreach (var user in users)
            {
                sb.AppendLine($"|{user.FirstName}{new string(' ', 28 - user.FirstName.Length)}|{user.LastName}{new string(' ', 29 - user.LastName.Length)}|{user.Email}{new string(' ', 29 - user.Email.Length)}|");
            }
            sb.AppendLine(new string('-', 90));
            return sb.ToString().Trim();
        }
    }
}