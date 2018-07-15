namespace BillsPaymentSystem.Services
{
    using Contracts;
    using Data;
    using Models;

    public class CreditCardService : ICreditCardService
    {
        private readonly BillsPaymentSystemContext dbContext;

        public CreditCardService(BillsPaymentSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Deposit(CreditCard creditCard, decimal amount)
        {
            creditCard.Deposit(amount);
            this.dbContext.SaveChanges();
        }

        public void Withdraw(CreditCard creditCard, decimal amount)
        {
            creditCard.Withdraw(amount);
            this.dbContext.SaveChanges();
        }
    }
}