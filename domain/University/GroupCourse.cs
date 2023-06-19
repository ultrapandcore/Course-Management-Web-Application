namespace University
{
    public class GroupCourse
    {
        public int GroupId { get; set; }
        public int CourseId { get; set; }
        public Group Group { get; set; }
        public Course Course { get; set; }
    }
}
