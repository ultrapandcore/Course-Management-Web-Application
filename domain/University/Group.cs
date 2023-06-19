using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        public List<Student> Students { get; set; } = new();
        public List<Course> Courses { get; set; } = new();
    }

}
