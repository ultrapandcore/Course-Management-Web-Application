using System.ComponentModel.DataAnnotations;

namespace University
{
    public class MonthlySchedule
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string? Month { get; set; }
        public List<DailySchedule> Schedule { get; set; } = new();
        public int PersonId { get; set; }
        public Person? Person { get; set; }
    }

}