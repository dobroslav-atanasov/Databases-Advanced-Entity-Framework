namespace TeamBuilder.Models
{
    public class EventTeam
    {
        public EventTeam()
        {
        }

        public EventTeam(int eventId, int teamId)
        {
            this.EventId = eventId;
            this.TeamId = teamId;
        }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}