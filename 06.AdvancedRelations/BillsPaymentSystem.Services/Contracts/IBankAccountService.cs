namespace BillsPaymentSystem.Services.Contracts
{
    using Models;

    public interface IBankAccountService
    {
        void Deposit(BankAccount bankAccount, decimal amount);

        void Withdraw(BankAccount bankAccount, decimal amount);
    }
}