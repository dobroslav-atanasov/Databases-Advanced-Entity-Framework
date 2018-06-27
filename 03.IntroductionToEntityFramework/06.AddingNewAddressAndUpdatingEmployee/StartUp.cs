namespace AddingNewAddressAndUpdatingEmployee
{
    using System;
    using System.Linq;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                Address address = new Address()
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                Employee employee = db.Employees.SingleOrDefault(e => e.LastName == "Nakov");
                employee.Address = address;

                db.SaveChanges();

                var addresses = db.Employees
                    .OrderByDescending(e => e.AddressId)
                    .Select(e => new
                    {
                        Text = e.Address.AddressText
                    })
                    .Take(10)
                    .ToList();

                foreach (var ad in addresses)
                {
                    Console.WriteLine(ad.Text);
                }
            }
        }
    }
}