using System.ComponentModel.DataAnnotations;

namespace University
{
    public class Mark
    {
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int LectureId { get; set; }
        public Lecture? Lecture { get; set; }

        [Range(1, 5)]
        public int Grade { get; set; }
    }

}