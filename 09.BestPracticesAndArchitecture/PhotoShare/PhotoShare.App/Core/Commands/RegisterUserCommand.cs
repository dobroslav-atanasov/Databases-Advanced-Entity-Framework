namespace PhotoShare.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Data;
    using Utilities;

    public class RegisterUserCommand : ICommand
    {
        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(4, arguments);
            
            string username = arguments[0];
            string password = arguments[1];
            string repeatPassword = arguments[2];
            string email = arguments[3];

            PhotoShareContext dbContext = new PhotoShareContext();

            if (password != repeatPassword)
            {
                throw new ArgumentException(Constants.PasswordsDoNotMatch);
            }

            if (Helper.IsUsernameExist(dbContext, username))
            {
                throw new InvalidOperationException(string.Format(Constants.UsernameIsTaken, username));
            }

            User user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now,
                LastTimeLoggedIn = DateTime.Now
            };
            
            using (dbContext)
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }

            return string.Format(Constants.RegisterUser, username);
        }
    }
}