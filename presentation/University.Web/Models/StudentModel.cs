using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace University.Web.Models
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You forgot to enter a First Name")]
        [StringLength(25, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You forgot to enter a Last Name")]
        [StringLength(25, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "You forgot to enter a Phone number")]
        [Phone]
        [Remote("IsPhoneAvailable", "Students", AdditionalFields = "Id")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Remote("IsEmailAvailable", "Students", AdditionalFields = "Id")]
        public string? Email { get; set; }

        public int? GroupId { get; set; }

        public Group? Group { get; set; }

        [DisplayName("Average Mark")]
        public decimal? AverageMark { get; set; }
    }
}
