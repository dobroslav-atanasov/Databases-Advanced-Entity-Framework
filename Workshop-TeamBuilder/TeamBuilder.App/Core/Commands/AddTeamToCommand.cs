namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class AddTeamToCommand : ICommand
    {
        private readonly IEventTeamService eventTeamService;

        public AddTeamToCommand(IEventTeamService eventTeamService)
        {
            this.eventTeamService = eventTeamService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            AuthenticationManager.Authorize();

            string eventName = arguments[0];
            string teamName = arguments[1];
            User currentUser = AuthenticationManager.GetCurrentUser();
            this.eventTeamService.AddTeamToEvent(eventName, teamName, currentUser);

            return $"Team {teamName} added for {eventName}!";
        }
    }
}