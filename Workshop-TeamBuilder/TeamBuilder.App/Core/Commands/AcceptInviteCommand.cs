namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class AcceptInviteCommand : ICommand
    {
        private readonly IInvitationService invitationService;

        public AcceptInviteCommand(IInvitationService invitationService)
        {
            this.invitationService = invitationService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);
            AuthenticationManager.Authorize();

            string teamName = arguments[0];
            User currentUser = AuthenticationManager.GetCurrentUser();
            this.invitationService.AcceptInvite(teamName, currentUser);

            return $"User {currentUser.Username} joined team {teamName}!";
        }
    }
}