namespace BillsPaymentSystem.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class UserService : IUserService
    {
        private readonly BillsPaymentSystemContext dbContext;

        public UserService(BillsPaymentSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User ById(int userId)
        {
            User user = this.dbContext
                .Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new InvalidOperationException(string.Format(ErrorMessage.UserDoesNotExist, userId));
            }

            return user;
        }

        public User ByEmailAndPassword(string email, string password)
        {
            User user = this.dbContext
                .Users
                .SingleOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                throw new InvalidOperationException(string.Format(ErrorMessage.InvalidEmailOrPassword));
            }

            return user;
        }

        public User CreateUser(string firstName, string lastName, string email, string password)
        {
            User user = new User(firstName, lastName, email, password);

            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();

            return user;
        }

        public void DeleteUser(int userId)
        {
            User user = this.dbContext.Users.Find(userId);

            if (user == null)
            {
                throw new InvalidOperationException(string.Format(ErrorMessage.UserDoesNotExist, userId));
            }

            this.dbContext.Users.Remove(user);
            this.dbContext.SaveChanges();
        }

        public User[] ListAllUsers()
        {
            User[] users = this.dbContext
                .Users
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToArray();

            return users;
        }
    }
}