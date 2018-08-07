namespace TeamBuilder.Services.Contracts
{
    using Models;

    public interface IEventService
    {
        Event CreateEvent(string[] arguments, User creator);

        string ShowEvent(string eventName);
    }
}