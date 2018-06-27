namespace DeleteProjectById
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
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                Project projectToDelete = db.Projects.Find(1);

                List<EmployeeProject> employees = db.EmployeesProjects
                    .Where(ep => ep.ProjectId == projectToDelete.ProjectId)
                    .ToList();

                db.EmployeesProjects.RemoveRange(employees);
                db.SaveChanges();

                db.Projects.Remove(projectToDelete);
                db.SaveChanges();

                var projects = db.Projects
                    .Select(p => new
                    {
                        p.Name
                    })
                    .Take(10)
                    .ToList();

                foreach (var pr in projects)
                {
                    Console.WriteLine($"{pr.Name}");
                }
            }
        }
    }
}