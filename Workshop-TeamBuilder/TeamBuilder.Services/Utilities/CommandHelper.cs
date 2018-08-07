namespace TeamBuilder.Services.Utilities
{
    using Data;
    using Models;
    using System.Linq;

    public static class CommandHelper
    {
        public static bool IsTeamExisting(TeamBuilderContext dbContext, string teamName)
        {
            return dbContext
                .Teams
                .Any(t => t.Name == teamName);
        }

        public static bool IsUserExisting(TeamBuilderContext dbContext, string username)
        {
            return dbContext
                .Users
                .Any(u => u.Username == username);
        }

        public static bool IsInviteExisting(TeamBuilderContext dbContext, string teamName, User user)
        {
            return dbContext
                .Invitations
                .Any(i => i.Team.Name == teamName && i.InvitedUserId == user.Id && i.IsActive);
        }

        public static bool IsUserCreatorOfTeam(TeamBuilderContext dbContext, string teamName, User user)
        {
            return dbContext
                .Teams
                .Any(t => t.Name == teamName && t.CreatorId == user.Id);
        }

        public static bool IsUserCreatorOfEvent(TeamBuilderContext dbContext, string eventName, User user)
        {
            return dbContext
                .Events
                .Any(e => e.Name == eventName && e.CreatorId == user.Id);
        }

        public static bool IsMemberOfTeam(TeamBuilderContext dbContext, string teamName, string username)
        {
            return dbContext
                .UserTeams
                .Any(ut => ut.User.Username == username && ut.Team.Name == teamName);
        }

        public static bool IsEventExisting(TeamBuilderContext dbContext, string eventName)
        {
            return dbContext
                .Events
                .Any(e => e.Name == eventName);
        }
    }
}