namespace TeamBuilder.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;
    using Utilities;

    public class UserService : IUserService
    {
        private readonly TeamBuilderContext dbContext;

        public UserService(TeamBuilderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User RegisterUser(string[] arguments)
        {
            string username = arguments[0];
            if (username.Length < Constants.MinUsernameLength || username.Length > Constants.MaxUsernameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UsernameNotValid, username));
            }

            string password = arguments[1];
            if (password.Length < Constants.MinPasswordLength 
                || password.Length > Constants.MaxPasswordLength 
                || !password.Any(char.IsDigit) 
                || !password.Any(char.IsUpper))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.PasswordNotValid, password));
            }

            string repeatPassword = arguments[2];
            if (password != repeatPassword)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.PasswordDoesNotMatch);
            }

            string firstName = arguments[3];
            string lastName = arguments[4];

            if (firstName.Length > Constants.MaxFirstNameLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.FirstNameNotValid, firstName);
            }

            if (lastName.Length > Constants.MaxLastNameLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.LastNameNotValid, lastName);
            }

            int age;
            bool isNumber = int.TryParse(arguments[5], out age);

            if (!isNumber || age <= 0)
            {
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            }

            Gender gender;
            bool isValidGender = Enum.TryParse(arguments[6], out gender);

            if (!isValidGender)
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            }

            if (CommandHelper.IsUserExisting(this.dbContext, username))
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsTaken, username));
            }

            User user = new User(username, password, firstName, lastName, age, gender);
            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();

            return user;
        }

        public User GetUserByCredentials(string username, string password)
        {
            User user = this.dbContext
                .Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            return user;
        }

        public void DeleteUser(User user)
        {
            this.dbContext.Users.Remove(user);
            this.dbContext.SaveChanges();
        }

        public User GetUserByUsername(string username)
        {
            User user = this.dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }

            return user;
        }
    }
}