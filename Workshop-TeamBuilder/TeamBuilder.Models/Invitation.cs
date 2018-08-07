namespace TeamBuilder.Models
{
    public class Invitation
    {
        public Invitation()
        {
            this.IsActive = true;
        }

        public Invitation(int invitedUserId, int teamId)
            : this()
        {
            this.InvitedUserId = invitedUserId;
            this.TeamId = teamId;
        }

        public int Id { get; set; }

        public int InvitedUserId { get; set; }
        public virtual User InvitedUser { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public bool IsActive { get; set; }
    }
}