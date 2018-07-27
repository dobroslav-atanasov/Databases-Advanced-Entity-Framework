namespace BusTicketsSystem.Services
{
    using System.Linq;
    using Contracts;
    using Data;
    using Models;

    public class BusCompanyService : IBusCompanyService
    {
        private readonly BusTicketsSystemContext dbContext;

        public BusCompanyService(BusTicketsSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public BusCompany ById(int id)
        {
            BusCompany busCompany = this.dbContext
                .BusCompanies
                .FirstOrDefault(bc => bc.Id == id);

            return busCompany;
        }

        public BusCompany ByName(string name)
        {
            BusCompany busCompany = this.dbContext
                .BusCompanies
                .FirstOrDefault(bc => bc.Name == name);

            return busCompany;
        }

        public bool Exists(int id)
        {
            bool isExist = this.dbContext
                .BusCompanies
                .Any(bc => bc.Id == id);

            return isExist;
        }

        public bool Exists(string name)
        {
            bool isExist = this.dbContext
                .BusCompanies
                .Any(bc => bc.Name == name);

            return isExist;
        }

        public void Add(string name)
        {
            BusCompany busCompany = new BusCompany()
            {
                Name = name
            };

            this.dbContext.BusCompanies.Add(busCompany);

            this.dbContext.SaveChanges();
        }
    }
}