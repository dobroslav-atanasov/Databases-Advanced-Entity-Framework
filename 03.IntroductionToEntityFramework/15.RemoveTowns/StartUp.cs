namespace RemoveTowns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            Console.Write("Insert town to delete: ");
            string townToDelete = Console.ReadLine();

            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                Town town = db.Towns.SingleOrDefault(t => t.Name == townToDelete);
                List<Address> addresses = db.Addresses
                    .Where(a => a.TownId == town.TownId)
                    .ToList();

                foreach (Employee employee in db.Employees)
                {
                    if (addresses.Contains(employee.Address))
                    {
                        employee.Address = null;
                    }
                }

                db.Addresses.RemoveRange(addresses);
                db.Towns.Remove(town);
                db.SaveChanges();

                if (addresses.Count == 1)
                {
                    Console.WriteLine($"1 address in {townToDelete} was deleted");
                }
                else
                {
                    Console.WriteLine($"{addresses.Count} addresses in {townToDelete} was deleted");
                }
            }
        }
    }
}