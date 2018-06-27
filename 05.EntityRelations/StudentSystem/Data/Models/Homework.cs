namespace P01_StudentSystem.Data.Models
{
    using System;
    using Enum;

    public class Homework
    {
        public Homework()
        {
        }

        public Homework(string content, ContentType contentType, DateTime submissionTime, Student student, Course course)
        {
            this.Content = content;
            this.ContentType = contentType;
            this.SubmissionTime = submissionTime;
            this.Student = student;
            this.Course = course;
        }

        public int HomeworkId { get; set; }

        public string Content { get; set; }

        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}