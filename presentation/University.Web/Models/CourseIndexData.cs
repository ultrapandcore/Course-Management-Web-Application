namespace University.Web.Models
{
    public class CourseIndexData
    {
        public List<Course> Courses { get; set; } = new();
        public List<Group> Groups { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public int? SelecterCourseId { get; set; }
        public int? SelectedGroupId { get; set; }
    }
}
