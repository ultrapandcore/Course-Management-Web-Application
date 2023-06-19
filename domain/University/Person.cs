using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string? LastName { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }
    }

}