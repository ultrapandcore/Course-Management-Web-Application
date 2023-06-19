using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace University.Web.Models
{
    public class GroupModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        [Remote("IsGroupNameAvailable", "Groups", AdditionalFields = "Id")]
        public string Name { get; set; }

        public List<Student> Students { get; set; } = new();

        public List<Course> Courses { get; set; } = new();
    }
}
