namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class LoginCommand : ICommand
    {
        // Login <username> <password>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);

            string username = arguments[0];
            string password = arguments[1];

            PhotoShareContext dbContext = new PhotoShareContext();

            User user = dbContext
                    .Users
                    .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                throw new ArgumentException(Constants.InvalidUsernameOrPassword);
            }

            if (Authentication.CurrentUser != null)
            {
                throw new ArgumentException(Constants.AlreadyLogged);
            }

            Authentication.CurrentUser = user;

            return string.Format(Constants.Login, username);
        }
    }
}