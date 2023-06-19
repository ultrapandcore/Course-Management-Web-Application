using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace University.Web.Models
{
    public class CourseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You forgot to enter a course name")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        [Remote("IsCourseNameAvailable", "Courses", AdditionalFields = "Id")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You forgot to enter a course description")]
        [StringLength(150, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 10)]
        public string Description { get; set; }

        public List<Group> Groups { get; set; } = new();
    }
}
