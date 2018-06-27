namespace FindLatest10Projects
{
    using System;
    using System.Globalization;
    using System.Linq;
    using P02_DatabaseFirst.Data;

    public class StartUp
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var projects = db.Projects
                    .Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.StartDate
                    })
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .ToList();

                foreach (var pr in projects.OrderBy(p => p.Name))
                {
                    Console.WriteLine($"{pr.Name}");
                    Console.WriteLine($"{pr.Description}");
                    Console.WriteLine($"{pr.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                }
            }
        }
    }
}