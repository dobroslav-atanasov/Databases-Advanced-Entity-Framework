namespace TeamBuilder.App.Core.Commands
{
    using System;
    using Contracts;
    using Data;
    using Models;
    using Services.Contracts;
    using Services.Utilities;
    using Utilities;

    public class InviteToTeamCommand : ICommand
    {
        private readonly ITeamService teamService;
        private readonly IUserService userService;
        private readonly IUserTeamService userTeamService;
        private readonly IInvitationService invitationService;

        public InviteToTeamCommand(ITeamService teamService, IUserService userService, IUserTeamService userTeamService, IInvitationService invitationService)
        {
            this.teamService = teamService;
            this.userService = userService;
            this.userTeamService = userTeamService;
            this.invitationService = invitationService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            AuthenticationManager.Authorize();

            string teamName = arguments[0];
            string username = arguments[1];
            User currentUser = AuthenticationManager.GetCurrentUser();

            Team team = this.teamService.GetTeamByName(teamName);
            User inviteUser = this.userService.GetUserByUsername(username);

            if (inviteUser == team.Creator)
            {
                this.userTeamService.AddUserToTeam(inviteUser, team);
                return $"Team {team.Name} invited {inviteUser.Username}!";
            }

            this.invitationService.SendInvitation(team, inviteUser, currentUser);

            return $"Team {teamName} invited {username}!";
        }
    }
}