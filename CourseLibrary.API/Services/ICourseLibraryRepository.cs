using CourseLibrary.API.Entities;

namespace CourseLibrary.API.Services
{
    public interface ICourseLibraryRepository
    {
        Task AddAuthorAsync(Author Author);
        Task AddCourseAsync(Guid AuthorId, Course course);
        Task<Author> GetAuthorAsync(Guid AuthorId);
        Task<IEnumerable<Author>> GetAuthorsAsync(string? name, string? search, int pageSize, int pageNumber);
        Task<Course> GetCourseAsync(Guid id, Guid AuthorId);
        Task<IEnumerable<Course>> GetCoursesOfInterestAsync(Guid AuthorId);
        Task<bool> IsSavedChanges();
        Task RemoveAuthor(Guid id);
        Task RemoveCourse(Guid AuthorId, Guid id);
    }
}