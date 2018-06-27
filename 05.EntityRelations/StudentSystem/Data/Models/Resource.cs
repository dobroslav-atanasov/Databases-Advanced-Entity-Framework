namespace P01_StudentSystem.Data.Models
{
    using Enum;

    public class Resource
    {
        public Resource()
        {
        }

        public Resource(string name, string url, ResourceType resourceType, Course course)
        {
            this.Name = name;
            this.Url = url;
            this.ResourceType = resourceType;
            this.Course = course;
        }

        public int ResourceId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}