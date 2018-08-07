namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class DisbandCommand : ICommand
    {
        private readonly ITeamService teamService;

        public DisbandCommand(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            AuthenticationManager.Authorize();

            string teamName = arguments[0];
            User currentUser = AuthenticationManager.GetCurrentUser();
            this.teamService.DeleteTeam(teamName, currentUser);

            return $"{teamName} has disbanded!";
        }
    }
}