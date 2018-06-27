namespace P02_DatabaseFirst.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Project
    {
        public Project()
        {
        }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<EmployeeProject> EmployeesProjects { get; set; } = new HashSet<EmployeeProject>();
    }
}