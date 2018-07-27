namespace BusTicketsSystem.Services
{
    using System.Linq;
    using Contracts;
    using Data;
    using Models;

    public class BankAccountService : IBankAccountService
    {
        private readonly BusTicketsSystemContext dbContext;

        public BankAccountService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public BankAccount ById(int id)
        {
            BankAccount bankAccount = this.dbContext
                .BankAccounts
                .FirstOrDefault(ba => ba.Id == id);

            return bankAccount;
        }

        public BankAccount ByCustomerId(int customerId)
        {
            BankAccount bankAccount = this.dbContext
                .BankAccounts
                .FirstOrDefault(ba => ba.CustomerId == customerId);

            return bankAccount;
        }

        public BankAccount ByIdAndCustomerId(int id, int customerId)
        {
            BankAccount bankAccount = this.dbContext
                .BankAccounts
                .FirstOrDefault(ba => ba.Id == id && ba.CustomerId == customerId);

            return bankAccount;
        }

        public bool Exists(int id)
        {
            bool isExist = this.dbContext
                .BankAccounts
                .Any(ba => ba.Id == id);

            return isExist;
        }

        public bool ExistsByCustomerId(int customerId)
        {
            bool isExist = this.dbContext
                .BankAccounts
                .Any(ba => ba.CustomerId == customerId);

            return isExist;
        }

        public void Add(int customerId, decimal amount)
        {
            BankAccount bankAccount = new BankAccount()
            {
                CustomerId = customerId,
                Balance = amount
            };

            this.dbContext.BankAccounts.Add(bankAccount);

            this.dbContext.SaveChanges();
        }

        public void Deposit(int customerId, decimal amount)
        {
            BankAccount bankAccount = this.ByCustomerId(customerId);

            bankAccount.Balance += amount;

            this.dbContext.SaveChanges();
        }

        public void Withdraw(int customerId, decimal amount)
        {
            BankAccount bankAccount = this.ByCustomerId(customerId);

            bankAccount.Balance -= amount;

            this.dbContext.SaveChanges();
        }
    }
}