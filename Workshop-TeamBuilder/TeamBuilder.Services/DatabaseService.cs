namespace TeamBuilder.Services
{
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseService : IDatabaseService
    {
        private readonly TeamBuilderContext dbContext;

        public DatabaseService(TeamBuilderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeDatabase()
        {
            this.dbContext.Database.Migrate();
        }
    }
}