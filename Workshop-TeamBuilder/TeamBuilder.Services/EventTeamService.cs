namespace TeamBuilder.Services
{
    using System;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class EventTeamService : IEventTeamService
    {
        private readonly TeamBuilderContext dbContext;

        public EventTeamService(TeamBuilderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddTeamToEvent(string eventName, string teamName, User currentUser)
        {
            Event @event = this.dbContext
                .Events
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefault(e => e.Name == eventName);

            if (@event == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            Team team = this.dbContext
                .Teams
                .FirstOrDefault(t => t.Name == teamName);

            if (team == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (CommandHelper.IsUserCreatorOfTeam(this.dbContext, teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            EventTeam eventTeam = this.dbContext
                .EventTeams
                .FirstOrDefault(et => et.TeamId == team.Id && et.EventId == @event.Id);

            if (eventTeam != null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
            }

            EventTeam newEventTeam = new EventTeam(@event.Id, team.Id);
            this.dbContext.EventTeams.Add(newEventTeam);
            this.dbContext.SaveChanges();
        }
    }
}