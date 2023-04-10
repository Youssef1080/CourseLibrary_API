using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    public class CourseToAdd
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1500)]
        public string? Description { get; set; }
    }
}