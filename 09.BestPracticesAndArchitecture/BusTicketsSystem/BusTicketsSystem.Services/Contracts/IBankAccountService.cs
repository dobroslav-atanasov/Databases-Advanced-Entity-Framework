namespace BusTicketsSystem.Services.Contracts
{
    using Models;

    public interface IBankAccountService
    {
        BankAccount ById(int id);

        BankAccount ByCustomerId(int customerId);

        BankAccount ByIdAndCustomerId(int id, int customerId);

        bool Exists(int id);

        bool ExistsByCustomerId(int customerId);

        void Add(int customerId, decimal amount);

        void Deposit(int customerId, decimal amount);

        void Withdraw(int customerId, decimal amount);
    }
}