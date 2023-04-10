using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    public class CourseDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1500)]
        public string? Description { get; set; }

        public Guid AuthorId { get; set; }
    }
}