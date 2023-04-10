using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Entities
{
    public class Course
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1500)]
        public string? Description { get; set; }

        public Author Author { get; set; }
        public Guid AuthorId { get; set; }
    }
}