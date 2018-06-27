namespace DepartmentsWithMoreThan5Employees
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
                var departments = db.Departments
                    .Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .Select(d => new
                    {
                        DepName = d.Name,
                        DepManagerFirstName = d.Manager.FirstName,
                        DepManagerLastName = d.Manager.LastName,
                        Employees = d.Employees
                            .Select(de => new
                            {
                                EmpFirstName = de.FirstName,
                                EmpLastName = de.LastName,
                                EmpJobTitle = de.JobTitle
                            })
                    })
                    .ToList();

                foreach (var dep in departments)
                {
                    Console.WriteLine($"{dep.DepName} - {dep.DepManagerFirstName} {dep.DepManagerLastName}");
                    foreach (var emp in dep.Employees.OrderBy(e => e.EmpFirstName).ThenBy(e => e.EmpLastName))
                    {
                        Console.WriteLine($"{emp.EmpFirstName} {emp.EmpLastName} - {emp.EmpJobTitle}");
                    }
                    Console.WriteLine(new string('-', 10));
                }
            }
        }
    }
}