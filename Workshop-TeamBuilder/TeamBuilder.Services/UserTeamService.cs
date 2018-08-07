namespace TeamBuilder.Services
{
    using System;
    using Contracts;
    using Data;
    using Models;
    using Utilities;

    public class UserTeamService : IUserTeamService
    {
        private readonly TeamBuilderContext dbContext;

        public UserTeamService(TeamBuilderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddUserToTeam(User user, Team team)
        {
            if (CommandHelper.IsMemberOfTeam(this.dbContext, team.Name, user.Username))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            UserTeam userTeam = new UserTeam(user.Id, team.Id);

            this.dbContext.UserTeams.Add(userTeam);
            this.dbContext.SaveChanges();
        }
    }
}