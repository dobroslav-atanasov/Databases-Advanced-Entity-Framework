namespace TeamBuilder.Services
{
    using System;
    using System.Linq;
    using System.Text;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class TeamService : ITeamService
    {
        private readonly TeamBuilderContext dbContext;

        public TeamService(TeamBuilderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Team CreateTeam(string[] arguments, User creator)
        {
            string name = arguments[0];
            if (name.Length > Constants.MaxTeamNameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNameInvalid, name));
            }

            string acronym = arguments[1];
            if (acronym.Length != Constants.TeamAcronymLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, acronym));
            }

            string description = arguments[2];
            if (description.Length > Constants.MaxTeamDescriptionLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamDescriptionInvalid, description));
            }

            if (CommandHelper.IsTeamExisting(this.dbContext, name))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, name));
            }

            Team team = new Team(name, acronym, description, creator.Id);
            this.dbContext.Teams.Add(team);
            this.dbContext.SaveChanges();

            return team;
        }

        public Team GetTeamByName(string name)
        {
            Team team = this.dbContext.Teams.FirstOrDefault(t => t.Name == name);

            if (team == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, name));
            }

            return team;
        }

        public void KickMember(string teamName, string username, User currentUser)
        {
            Team team = this.GetTeamByName(teamName);

            User user = this.dbContext
                .Users
                .FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, username));
            }

            if (!CommandHelper.IsMemberOfTeam(this.dbContext, teamName, username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.NotPartOfTeam, username, teamName));
            }

            if (!CommandHelper.IsUserCreatorOfTeam(this.dbContext, teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            User creator = team.Creator;
            if (user == creator)
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.CommandNotAllowed, teamName));
            }

            UserTeam userTeam = this.dbContext
                .UserTeams
                .FirstOrDefault(ut => ut.TeamId == team.Id && ut.UserId == user.Id);
            this.dbContext.UserTeams.Remove(userTeam);
            this.dbContext.SaveChanges();
        }

        public void DeleteTeam(string teamName, User currentUser)
        {
            Team team = this.GetTeamByName(teamName);

            if (!CommandHelper.IsUserCreatorOfTeam(this.dbContext, teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            UserTeam[] usersTeams = this.dbContext
                .UserTeams
                .Where(ut => ut.TeamId == team.Id)
                .ToArray();

            Invitation[] invitations = this.dbContext
                .Invitations
                .Where(i => i.TeamId == team.Id)
                .ToArray();

            EventTeam[] eventsTeams = this.dbContext
                .EventTeams
                .Where(i => i.TeamId == team.Id)
                .ToArray();

            this.dbContext.EventTeams.RemoveRange(eventsTeams);
            this.dbContext.Invitations.RemoveRange(invitations);
            this.dbContext.UserTeams.RemoveRange(usersTeams);
            this.dbContext.Teams.Remove(team);
            this.dbContext.SaveChanges();
        }

        public string ShowTeam(string teamName)
        {
            Team team = this.GetTeamByName(teamName);

            User[] members = this.dbContext
                .UserTeams
                .Where(ut => ut.TeamId == team.Id)
                .Select(ut => ut.User)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Team: {team.Name} | Acronym: {team.Acronym}");
            sb.AppendLine($"Members: ");
            foreach (User user in members)
            {
                sb.AppendLine($" -- {user.Username}");
            }

            return sb.ToString().Trim();
        }
    }
}