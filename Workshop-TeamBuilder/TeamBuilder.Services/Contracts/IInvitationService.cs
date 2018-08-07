namespace TeamBuilder.Services.Contracts
{
    using Models;

    public interface IInvitationService
    {
        void SendInvitation(Team team, User inviteUser, User currentUser);

        void AcceptInvite(string teamName, User currentUser);

        void DeclineInvite(string teamName, User currentUser);
    }
}