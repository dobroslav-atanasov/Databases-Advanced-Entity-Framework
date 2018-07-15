namespace BillsPaymentSystem.Services
{
    using Contracts;
    using Data;
    using Models;

    public class BankAccountService : IBankAccountService
    {
        private readonly BillsPaymentSystemContext dbContext;

        public BankAccountService(BillsPaymentSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Deposit(BankAccount bankAccount, decimal amount)
        {
            bankAccount.Deposit(amount);
            this.dbContext.SaveChanges();
        }

        public void Withdraw(BankAccount bankAccount, decimal amount)
        {
            bankAccount.Withdraw(amount);
            this.dbContext.SaveChanges();
        }
    }
}