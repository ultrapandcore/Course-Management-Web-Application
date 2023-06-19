using System.ComponentModel.DataAnnotations;

namespace University
{
    public class Lecture
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public LectureRoom? LectureRoom { get; set; }
        public Group? Group { get; set; }
        public Teacher? Teacher { get; set; }
        public Course? Course { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }
    }

}