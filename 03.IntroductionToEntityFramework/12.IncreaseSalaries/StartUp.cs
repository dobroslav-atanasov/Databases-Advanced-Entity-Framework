namespace IncreaseSalaries
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
            const decimal salaryIncrease = 1.12m;

            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                List<Employee> employess = db.Employees
                    .Where(e => e.Department.Name == "Engineering"
                                || e.Department.Name == "Tool Design"
                                || e.Department.Name == "Marketing"
                                || e.Department.Name == "Information Services")
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList();
                
                foreach (Employee emp in employess)
                {
                    emp.Salary *= salaryIncrease;
                    Console.WriteLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:F2})");
                }

                db.SaveChanges();
            }
        }
    }
}