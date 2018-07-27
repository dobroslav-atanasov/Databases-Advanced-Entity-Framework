namespace PhotoShare.App.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class ModifyUserCommand : ICommand
    {
        // ModifyUser <username> <property> <new value>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(3, arguments);
            Authentication.Authorize();

            string username = arguments[0];
            string property = arguments[1];
            string newValue = arguments[2];

            PhotoShareContext dbContext = new PhotoShareContext();

            if (!Helper.IsUsernameExist(dbContext, username))
            {
                throw new ArgumentException(string.Format(Constants.UserNotFound, username));
            }

            if (!Helper.IsValidProperty(property))
            {
                throw new ArgumentException(string.Format(Constants.PropertyNotFount, property));
            }

            switch (property)
            {
                case "Password":
                    this.ModifyUserPassword(dbContext, username, newValue);
                    break;
                case "BornTown":
                    this.ModifyUserBornTown(dbContext, username, newValue);
                    break;
                case "CurrentTown":
                    this.ModifyUserCurrentTown(dbContext, username, newValue);
                    break;
            }

            return string.Format(Constants.ModifyUser, username, property, newValue);
        }

        private void ModifyUserCurrentTown(PhotoShareContext dbContext, string username, string newValue)
        {
            using (dbContext)
            {
                User user = this.GetUserByUsername(dbContext, username);
                Town town = this.GetTownByName(dbContext, newValue);

                if (town == null)
                {
                    throw new ArgumentException(string.Format(Constants.TownDoesNotExist, newValue));
                }

                user.CurrentTownId = town.Id;
                dbContext.SaveChanges();
            }
        }

        private void ModifyUserBornTown(PhotoShareContext dbContext, string username, string newValue)
        {
            using (dbContext)
            {
                User user = this.GetUserByUsername(dbContext, username);
                Town town = this.GetTownByName(dbContext, newValue);

                if (town == null)
                {
                    throw new ArgumentException(string.Format(Constants.TownDoesNotExist, newValue));
                }

                user.BornTownId = town.Id;
                dbContext.SaveChanges();
            }
        }

        private Town GetTownByName(PhotoShareContext dbContext, string newValue)
        {
            Town town = dbContext
                .Towns
                .FirstOrDefault(t => t.Name == newValue);

            return town;
        }

        private void ModifyUserPassword(PhotoShareContext dbContext, string username, string newValue)
        {
            using (dbContext)
            {
                User user = this.GetUserByUsername(dbContext, username);

                if (!newValue.Any(char.IsDigit) || !newValue.Any(char.IsUpper))
                {
                    throw new ArgumentException(Constants.InvalidPassword);
                }

                user.Password = newValue;
                dbContext.SaveChanges();
            }
        }

        private User GetUserByUsername(PhotoShareContext dbContext, string username)
        {
            User user = dbContext
                .Users
                .FirstOrDefault(u => u.Username == username);

            return user;
        }
    }
}