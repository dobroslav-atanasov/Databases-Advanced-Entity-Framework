namespace TeamBuilder.Services.Contracts
{
    using Models;

    public interface IUserService
    {
        User RegisterUser(string[] arguments);

        User GetUserByCredentials(string username, string password);

        void DeleteUser(User user);

        User GetUserByUsername(string username);
    }
}