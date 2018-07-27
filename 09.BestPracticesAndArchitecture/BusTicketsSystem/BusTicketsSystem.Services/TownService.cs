namespace BusTicketsSystem.Services
{
    using Contracts;
    using Data;
    using Models;
    using System.Linq;

    public class TownService : ITownService
    {
        private readonly BusTicketsSystemContext dbContext;

        public TownService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Town ById(int id)
        {
            Town town = this.dbContext
                .Towns
                .FirstOrDefault(t => t.Id == id);

            return town;
        }

        public Town ByName(string name)
        {
            Town town = this.dbContext
                .Towns
                .FirstOrDefault(t => t.Name == name);

            return town;
        }

        public bool Exists(int id)
        {
            bool isExist = this.dbContext
                .Towns
                .Any(bc => bc.Id == id);

            return isExist;
        }

        public bool Exists(string name)
        {
            bool isExist = this.dbContext
                .Towns
                .Any(bc => bc.Name == name);

            return isExist;
        }

        public void Add(string townName, string country)
        {
            Town town = new Town()
            {
                Name = townName,
                Country = country
            };

            this.dbContext.Towns.Add(town);

            this.dbContext.SaveChanges();
        }
    }
}