namespace TeamBuilder.Models
{
    public class UserTeam
    {
        public UserTeam()
        {
        }

        public UserTeam(int userId, int teamId)
        {
            this.UserId = userId;
            this.TeamId = teamId;
        }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}