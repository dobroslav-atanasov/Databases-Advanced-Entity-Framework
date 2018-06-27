namespace AddressesByTown
{
    using System;
    using System.Linq;
    using P02_DatabaseFirst.Data;

    public class StartUp
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var addresses = db.Addresses
                    .OrderByDescending(a => a.Employees.Count)
                    .ThenBy(a => a.Town.Name)
                    .ThenBy(a => a.AddressText)
                    .Select(a => new
                    {
                        a.AddressText,
                        TownName = a.Town.Name,
                        EmployeesCount = a.Employees.Count
                    })
                    .Take(10)
                    .ToList();

                foreach (var ad in addresses)
                {
                    Console.WriteLine($"{ad.AddressText}, {ad.TownName} - {ad.EmployeesCount} employees");
                }
            }
        }
    }
}