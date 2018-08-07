namespace TeamBuilder.Services.Contracts
{
    using Models;

    public interface ITeamService
    {
        Team CreateTeam(string[] arguments, User creator);

        void KickMember(string teamName, string username, User currentUser);

        void DeleteTeam(string teamName, User currentUser);

        Team GetTeamByName(string name);

        string ShowTeam(string teamName);
    }
}