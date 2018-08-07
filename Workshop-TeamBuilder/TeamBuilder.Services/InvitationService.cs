namespace TeamBuilder.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class InvitationService : IInvitationService
    {
        private readonly TeamBuilderContext dbContext;

        public InvitationService(TeamBuilderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SendInvitation(Team team, User inviteUser, User currentUser)
        {
            if (currentUser != team.Creator || CommandHelper.IsMemberOfTeam(this.dbContext, team.Name, inviteUser.Username))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (CommandHelper.IsInviteExisting(this.dbContext, team.Name, inviteUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }

            Invitation invitation = new Invitation(inviteUser.Id, team.Id);
            this.dbContext.Invitations.Add(invitation);
            this.dbContext.SaveChanges();
        }

        public void AcceptInvite(string teamName, User currentUser)
        {
            Team team = this.dbContext
                .Teams
                .FirstOrDefault(t => t.Name == teamName);

            if (team == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            Invitation invitation = this.dbContext
                .Invitations
                .Where(i => i.IsActive)
                .FirstOrDefault(i => i.Team.Name == teamName && i.InvitedUser.Id == currentUser.Id);

            if (invitation == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            invitation.IsActive = false;

            UserTeam userTeam = new UserTeam(currentUser.Id, team.Id);
            team.UserTeams.Add(userTeam);
            this.dbContext.SaveChanges();
        }

        public void DeclineInvite(string teamName, User currentUser)
        {
            Team team = this.dbContext
                .Teams
                .FirstOrDefault(t => t.Name == teamName);

            if (team == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            Invitation invitation = this.dbContext
                .Invitations
                .Where(i => i.IsActive)
                .FirstOrDefault(i => i.Team.Name == teamName && i.InvitedUser.Id == currentUser.Id);

            if (invitation == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            invitation.IsActive = false;
            this.dbContext.SaveChanges();
        }
    }
}