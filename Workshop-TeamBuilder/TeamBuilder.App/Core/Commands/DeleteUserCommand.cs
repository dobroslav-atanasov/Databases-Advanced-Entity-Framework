namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class DeleteUserCommand : ICommand
    {
        private readonly IUserService userService;

        public DeleteUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(0, arguments);

            AuthenticationManager.Authorize();
            User user = AuthenticationManager.Logout();
            this.userService.DeleteUser(user);

            return $"User {user.Username} was deleted successfully!";
        }
    }
}