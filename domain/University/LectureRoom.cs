using System.ComponentModel.DataAnnotations;

namespace University
{
    public class LectureRoom
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        public int Capacity { get; set; }
    }

}