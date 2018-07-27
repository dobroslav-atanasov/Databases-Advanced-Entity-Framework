namespace BusTicketsSystem.Services
{
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseService : IDatabaseService
    {
        private readonly BusTicketsSystemContext dbContext;

        public DatabaseService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Initialize()
        {
            this.dbContext.Database.Migrate();
        }
    }
}