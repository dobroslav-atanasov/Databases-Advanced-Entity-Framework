namespace TeamBuilder.Services.Contracts
{
    using Models;

    public interface IEventTeamService
    {
        void AddTeamToEvent(string eventName, string teamName, User currentUser);
    }
}