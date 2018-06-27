namespace EmployeesAndProjects
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
                var employees = db.Employees
                    .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        ManagerFirstName = e.Manager.FirstName,
                        ManagerLastName = e.Manager.LastName,
                        Projects = e.EmployeesProjects
                            .Select(ep => new
                            {
                                ep.Project.Name,
                                ep.Project.StartDate,
                                ep.Project.EndDate
                            })
                    })
                    .Take(30)
                    .ToList();

                foreach (var emp in employees)
                {
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");
                    foreach (var pr in emp.Projects)
                    {
                        string startDate = pr.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        string endDate = pr.EndDate != null 
                            ? pr.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) 
                            : "not finished";
                        Console.WriteLine($"--{pr.Name} - {startDate} - {endDate}");
                    }
                }
            }
        }
    }
}