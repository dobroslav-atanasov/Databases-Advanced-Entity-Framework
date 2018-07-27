namespace BusTicketsSystem.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;

    public class CustomerService : ICustomerService
    {
        private readonly BusTicketsSystemContext dbContext;

        public CustomerService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Customer ById(int id)
        {
            Customer customer = this.dbContext
                .Customers
                .FirstOrDefault(c => c.Id == id);

            return customer;
        }

        public bool Exists(int id)
        {
            bool isExist = this.dbContext
                .Customers
                .Any(c => c.Id == id);

            return isExist;
        }

        public void Add(string firstName, string lastName, string genderString, int townId)
        {
            Gender gender = Enum.Parse<Gender>(genderString);

            Customer customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                HomeTownId = townId
            };

            this.dbContext.Customers.Add(customer);

            this.dbContext.SaveChanges();
        }
    }
}