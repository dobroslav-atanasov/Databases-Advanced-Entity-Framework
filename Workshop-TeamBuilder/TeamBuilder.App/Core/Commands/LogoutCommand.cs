namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Utilities;

    public class LogoutCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            Check.CheckLength(0, arguments);

            User user = AuthenticationManager.Logout();

            return $"User {user.Username} successfully logged out!";
        }
    }
}