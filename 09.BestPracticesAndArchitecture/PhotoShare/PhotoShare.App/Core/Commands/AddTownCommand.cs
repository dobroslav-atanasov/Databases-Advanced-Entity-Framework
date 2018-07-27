namespace PhotoShare.App.Core.Commands
{
    using System;
    using Contracts;
    using Models;
    using Data;
    using Utilities;

    public class AddTownCommand : ICommand
    {
        // AddTown <townName> <countryName>
        public string Execute(string[] arguments)
        {
            Check.CheckLength(2, arguments);
            Authentication.Authorize();
            string townName = arguments[0];
            string country = arguments[1];
            PhotoShareContext dbContext = new PhotoShareContext();

            if (Helper.IsTownNameExist(dbContext, townName))
            {
                throw new ArgumentException(string.Format(Constants.TownAlreadtExist, townName));
            }

            using (dbContext)
            {
                Town town = new Town
                {
                    Name = townName,
                    Country = country
                };

                dbContext.Towns.Add(town);
                dbContext.SaveChanges();
            }

            return string.Format(Constants.AddTown, townName);
        }
    }
}