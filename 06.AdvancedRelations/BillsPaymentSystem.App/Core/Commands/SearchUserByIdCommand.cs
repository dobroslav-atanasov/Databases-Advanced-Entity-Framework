namespace BillsPaymentSystem.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;

    public class SearchUserByIdCommand : ICommand
    {
        private readonly IUserService userService;

        public SearchUserByIdCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] arguments)
        {
            int userId = int.Parse(arguments[0]);
            User user = this.userService.ById(userId);

            return $"User with id {userId} is in the system!";
        }
    }
}