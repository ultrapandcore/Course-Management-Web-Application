using System.ComponentModel.DataAnnotations.Schema;

namespace University
{
    [Table("Teachers")]
    public class Teacher : Person
    {
        public List<Course> Courses { get; set; } = new();

        [NotMapped]
        public MonthlySchedule? Schedule { get; set; }
    }

}