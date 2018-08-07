namespace TeamBuilder.App.Core.Commands
{
    using Contracts;
    using Services.Contracts;
    using Utilities;

    public class ShowEventCommand : ICommand
    {
        private readonly IEventService eventService;

        public ShowEventCommand(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLength(1, arguments);

            string eventName = arguments[0];
            string result = this.eventService.ShowEvent(eventName);
            return result;
        }
    }
}