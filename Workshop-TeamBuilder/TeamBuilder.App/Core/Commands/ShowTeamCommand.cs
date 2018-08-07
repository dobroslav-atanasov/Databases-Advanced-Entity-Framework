namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Services.Contracts;
    using Utilities;

    public class ShowTeamCommand : ICommand
    {
        private readonly ITeamService teamService;

        public ShowTeamCommand(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);

            string teamName = arguments[0];
            string result = this.teamService.ShowTeam(teamName);

            return result;
        }
    }
}