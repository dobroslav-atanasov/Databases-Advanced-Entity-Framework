namespace BusTicketsSystem.Services
{
    using Contracts;
    using Data;
    using Models;
    using System.Linq;

    public class BusStationService : IBusStationService
    {
        private readonly BusTicketsSystemContext dbContext;

        public BusStationService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public BusStation ById(int id)
        {
            BusStation busStation = this.dbContext
                .BusStations
                .FirstOrDefault(bs => bs.Id == id);

            return busStation;
        }

        public BusStation ByName(string name)
        {
            BusStation busStation = this.dbContext
                .BusStations
                .FirstOrDefault(bs => bs.Name == name);

            return busStation;
        }

        public bool Exists(int id)
        {
            bool isExist = this.dbContext
                .BusStations
                .Any(bc => bc.Id == id);

            return isExist;
        }

        public bool Exists(string name)
        {
            bool isExist = this.dbContext
                .BusStations
                .Any(bc => bc.Name == name);

            return isExist;
        }

        public void Add(string name, int townId)
        {
            BusStation busStation = new BusStation()
            {
                Name = name,
                TownId = townId
            };

            this.dbContext.BusStations.Add(busStation);

            this.dbContext.SaveChanges();
        }
    }
}