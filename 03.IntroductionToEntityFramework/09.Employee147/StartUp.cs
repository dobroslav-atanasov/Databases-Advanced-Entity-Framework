namespace Employee147
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                var employee = db.Employees
                    .Select(e => new
                    {
                        e.EmployeeId,
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        Project = e.EmployeesProjects
                            .Select(ep => new
                            {
                                ProjectName = ep.Project.Name
                            })
                    })
                    .SingleOrDefault(e => e.EmployeeId == 147);

                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                foreach (var pr in employee.Project.OrderBy(p => p.ProjectName))
                {
                    Console.WriteLine($"{pr.ProjectName}");
                }
            }
        }
    }
}