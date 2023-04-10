using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    public class AuthorDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        [StringLength(50)]
        public string MainCategory { get; set; }

        public List<CourseDto> Courses { get; set; } = new List<CourseDto>();
    }
}