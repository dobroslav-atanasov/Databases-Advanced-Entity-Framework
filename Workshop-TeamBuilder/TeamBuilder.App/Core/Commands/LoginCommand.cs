namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class LoginCommand : ICommand
    {
        private readonly IUserService userService;

        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string username = arguments[0];
            string password = arguments[1];
            User user = this.userService.GetUserByCredentials(username, password);

            AuthenticationManager.Login(user);

            return $"User {username} successfully logged in!";
        }
    }
}