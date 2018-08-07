namespace TeamBuilder.Services
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class EventService : IEventService
    {
        private readonly TeamBuilderContext dbContext;

        public EventService(TeamBuilderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Event CreateEvent(string[] arguments, User creator)
        {
            string name = arguments[0];
            if (name.Length > Constants.MaxEventNameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNameNotValid, name));
            }

            string description = arguments[1];
            if (description.Length > Constants.MaxEventDescriptionLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.EventDescriptionNotValid);
            }

            string startDateTime = $"{arguments[2]} {arguments[3]}";
            DateTime startDate;
            bool isValidStartDate = DateTime.TryParseExact(startDateTime, Constants.DateTimeFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            if (!isValidStartDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            string endDateTime = $"{arguments[4]} {arguments[5]}";
            DateTime endDate;
            bool isValidEndDate = DateTime.TryParseExact(endDateTime, Constants.DateTimeFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

            if (!isValidEndDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            if (startDate >= endDate)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidStartAndEndDates);
            }

            Event @event = new Event(name, description, startDate, endDate, creator.Id);

            this.dbContext.Events.Add(@event);
            this.dbContext.SaveChanges();

            return @event;
        }

        public string ShowEvent(string eventName)
        {
            Event @event = this.dbContext
                .Events
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefault(e => e.Name == eventName);

            if (@event == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            Team[] teams = @event.EventTeams.Select(et => et.Team).ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Event name: {@event.Name} |Start date: {@event.StartDate} |End date: {@event.EndDate}");
            sb.AppendLine($"Teams: ");
            foreach (Team team in teams)
            {
                sb.AppendLine($"  - {team.Name}");
            }
            return sb.ToString().Trim();
        }
    }
}