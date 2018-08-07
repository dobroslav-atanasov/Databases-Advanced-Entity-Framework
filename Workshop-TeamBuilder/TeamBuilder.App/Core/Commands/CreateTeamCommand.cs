namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class CreateTeamCommand : ICommand
    {
        private readonly ITeamService teamService;

        public CreateTeamCommand(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(3, arguments);
            AuthenticationManager.Authorize();
            User creator = AuthenticationManager.GetCurrentUser();
            Team team = this.teamService.CreateTeam(arguments, creator);

            return $"Team {team.Name} successfully created!";
        }
    }
}