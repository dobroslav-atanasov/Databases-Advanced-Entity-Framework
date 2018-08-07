namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class KickMemberCommand : ICommand
    {
        private readonly ITeamService teamService;

        public KickMemberCommand(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            AuthenticationManager.Authorize();

            string teamName = arguments[0];
            string username = arguments[1];
            User currentUser = AuthenticationManager.GetCurrentUser();
            this.teamService.KickMember(teamName, username, currentUser);

            return $"User {username} was kicked from {teamName}!";
        }
    }
}