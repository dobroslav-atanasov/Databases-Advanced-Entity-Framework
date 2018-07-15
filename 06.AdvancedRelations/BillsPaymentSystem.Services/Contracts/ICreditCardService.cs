namespace BillsPaymentSystem.Services.Contracts
{
    using Models;

    public interface ICreditCardService
    {
        void Deposit(CreditCard creditCard, decimal amount);

        void Withdraw(CreditCard creditCard, decimal amount);
    }
}