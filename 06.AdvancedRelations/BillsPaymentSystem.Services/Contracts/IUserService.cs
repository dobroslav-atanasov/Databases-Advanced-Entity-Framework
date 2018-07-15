namespace BillsPaymentSystem.Services.Contracts
{
    using Data;
    using Models;

    public interface IUserService
    {
        User ById(int userId);

        User ByEmailAndPassword(string email, string password);

        User CreateUser(string firstName, string lastName, string email, string password);

        void DeleteUser(int userId);

        User[] ListAllUsers();
    }
}