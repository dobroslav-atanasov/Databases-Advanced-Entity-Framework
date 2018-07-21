namespace Employees.Services
{
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseService : IDatabaseService
    {
        private readonly EmployeeContext dbContext;

        public DatabaseService(EmployeeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeDatabase()
        {
            this.dbContext.Database.Migrate();
        }
    }
}