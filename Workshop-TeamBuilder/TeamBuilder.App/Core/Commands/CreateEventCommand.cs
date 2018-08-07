namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Models;
    using Services.Contracts;
    using Utilities;

    public class CreateEventCommand : ICommand
    {
        private readonly IEventService eventService;

        public CreateEventCommand(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(6, arguments);
            AuthenticationManager.Authorize();
            User creator = AuthenticationManager.GetCurrentUser();
            Event @event = this.eventService.CreateEvent(arguments, creator); 

            return $"Event {@event.Name} was created successfully!";
        }
    }
}