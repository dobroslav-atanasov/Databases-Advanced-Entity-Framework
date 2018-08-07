namespace TeamBuilder.Models
{
    using System;
    using System.Collections.Generic;

    public class Event
    {
        public Event()
        {
            this.EventTeams = new List<EventTeam>();
        }

        public Event(string name, string description, DateTime startDate, DateTime endDate, int creatorId)
            :this()
        {
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.CreatorId = creatorId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<EventTeam> EventTeams { get; set; }
    }
}