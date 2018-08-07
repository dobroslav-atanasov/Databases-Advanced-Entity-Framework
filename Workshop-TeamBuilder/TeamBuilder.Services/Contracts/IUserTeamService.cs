namespace TeamBuilder.Services.Contracts
{
    using Models;

    public interface IUserTeamService
    {
        void AddUserToTeam(User user, Team team);
    }
}