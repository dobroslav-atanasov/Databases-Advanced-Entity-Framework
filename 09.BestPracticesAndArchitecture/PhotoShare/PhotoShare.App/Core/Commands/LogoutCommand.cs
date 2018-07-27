namespace PhotoShare.App.Core.Commands
{
    using Contracts;
    using Models;
    using Utilities;

    public class LogoutCommand : ICommand
    {
        // Logout
        public string Execute(string[] arguments)
        {
            Check.CheckLength(0, arguments);
            Authentication.Authorize();

            User currentUser = Authentication.GetCurrentUser();
            Authentication.CurrentUser = null;

            return string.Format(Constants.Logout, currentUser.Username);
        }
    }
}