namespace TeamBuilder.Models
{
    using System.Collections.Generic;

    public class Team
    {
        public Team()
        {
            this.Invitations = new List<Invitation>();
            this.UserTeams = new List<UserTeam>();
            this.EventTeams = new List<EventTeam>();
        }

        public Team(string name, string acronym, string description, int creatorId)
            :this()
        {
            this.Name = name;
            this.Acronym = acronym;
            this.Description = description;
            this.CreatorId = creatorId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Acronym { get; set; }

        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<UserTeam> UserTeams { get; set; }

        public virtual ICollection<EventTeam> EventTeams { get; set; }
    }
}