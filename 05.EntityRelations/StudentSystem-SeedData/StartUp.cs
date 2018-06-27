namespace StudentSystem_SeedData
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data;
    using P01_StudentSystem.Data.Enum;
    using P01_StudentSystem.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            StudentSystemContext db = new StudentSystemContext();

            using (db)
            {
                db.Database.EnsureDeleted();
                db.Database.Migrate();

                SeedData(db);
            }
        }

        private static void SeedData(StudentSystemContext db)
        {
            Student[] students = AddStudents(db);
            Course[] courses = AddCourses(db);
            AddResources(db, courses);
            AddStudentCourses(db, students, courses);
            AddHomeworks(db, students, courses);

            db.SaveChanges();
        }

        private static void AddHomeworks(StudentSystemContext db, Student[] students, Course[] courses)
        {
            Homework[] homeworks = new Homework[]
            {
                new Homework("Stacks and Queues - Lab", ContentType.Application, DateTime.ParseExact("08-02-2018", "dd-MM-yyyy", null), students[0], courses[0]),
                new Homework("Joins and Subqueries - Exercises", ContentType.Application, DateTime.ParseExact("10-06-2018", "dd-MM-yyyy", null), students[1], courses[1]),
                new Homework("Entity Relations - Exercises", ContentType.Zip, DateTime.ParseExact("20-07-2018", "dd-MM-yyyy", null), students[2], courses[2]),
            };

            db.HomeworkSubmissions.AddRange(homeworks);
        }

        private static void AddStudentCourses(StudentSystemContext db, Student[] students, Course[] courses)
        {
            StudentCourse[] studentCourses = new StudentCourse[]
            {
                new StudentCourse(students[0], courses[0]),
                new StudentCourse(students[1], courses[1]),
                new StudentCourse(students[2], courses[2]),
            };

            db.StudentCourses.AddRange(studentCourses);
        }

        private static void AddResources(StudentSystemContext db, Course[] courses)
        {
            Resource[] resources = new Resource[]
            {
                new Resource("C# Fundamentals - Stacks and Queues", "www.softuni.com/c#fundamentals/labs", ResourceType.Document, courses[0]),
                new Resource("Database Basics - Joins and Subqueries", "www.softuni.com/databasebasics/presentations", ResourceType.Presentation, courses[1]),
                new Resource("Database Advanced - Entity Relations", "www.softuni.com/databaseadvanced/videos", ResourceType.Video, courses[2]),
            };

            db.Resources.AddRange(resources);
        }

        private static Course[] AddCourses(StudentSystemContext db)
        {
            Course[] courses = new Course[]
            {
                new Course("C# Fundamentals", DateTime.ParseExact("28-01-2018", "dd-MM-yyyy", null), DateTime.ParseExact("28-04-2018", "dd-MM-yyyy", null), 330.00m),
                new Course("Database Basics", DateTime.ParseExact("21-05-2018", "dd-MM-yyyy", null), DateTime.ParseExact("24-06-2018", "dd-MM-yyyy", null), 330.00m),
                new Course("Database Advanced", DateTime.ParseExact("25-06-2018", "dd-MM-yyyy", null), DateTime.ParseExact("12-08-2018", "dd-MM-yyyy", null), 330.00m),
            };

            db.Courses.AddRange(courses);
            return courses;
        }

        private static Student[] AddStudents(StudentSystemContext db)
        {
            Student[] students = new Student[]
            {
                new Student("Ivan", DateTime.ParseExact("12-06-2018", "dd-MM-yyyy", null))
                {
                    Birthday = DateTime.ParseExact("02-02-1990", "dd-MM-yyyy", null)
                },
                new Student("Petar", DateTime.ParseExact("06-06-2018", "dd-MM-yyyy", null)),
                new Student("Maria", DateTime.ParseExact("01-06-2018", "dd-MM-yyyy", null)),
            };

            db.Students.AddRange(students);
            return students;
        }
    }
}