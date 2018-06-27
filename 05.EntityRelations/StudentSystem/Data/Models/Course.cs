namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Course
    {
        public Course()
        {
            this.Resources = new List<Resource>();
            this.HomeworkSubmissions = new List<Homework>();
            this.StudentsEnrolled = new List<StudentCourse>();
        }

        public Course(string name, DateTime startDate, DateTime endDate, decimal price)
            :this()
        {
            this.Name = name;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Price = price;
        }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<Resource> Resources { get; set; }

        public ICollection<Homework> HomeworkSubmissions { get; set; }

        public ICollection<StudentCourse> StudentsEnrolled { get; set; }
    }
}