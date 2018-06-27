namespace EmployeesWithSalaryOver50000
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
                var employees = db.Employees
                    .Where(e => e.Salary > 50000)
                    .Select(e => new
                    {
                        e.FirstName
                    })
                    .OrderBy(e => e.FirstName)
                    .ToList();

                foreach (var emp in employees)
                {
                    Console.WriteLine(emp.FirstName);
                }
            }
        }
    }
}