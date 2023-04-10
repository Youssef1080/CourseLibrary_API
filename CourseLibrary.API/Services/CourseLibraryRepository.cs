using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.API.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository
    {
        private readonly CourseDbContext context;

        public CourseLibraryRepository(CourseDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync(string? name, string? search, int pageSize, int pageNumber)
        {
            var Author = await context.Authors.Include(c => c.Courses).ToListAsync();

            Author = Author.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return Author;
        }

        public async Task<Author> GetAuthorAsync(Guid AuthorId)
        {
            return await context.Authors.Where(c => c.Id == AuthorId).Include(c => c.Courses).FirstOrDefaultAsync();
        }

        public async Task AddAuthorAsync(Author Author)
        {
            await context.Authors.AddAsync(Author);
        }

        public async Task RemoveAuthor(Guid id)
        {
            var Author = await context.Authors.Where(c => c.Id == id).FirstOrDefaultAsync();
            context.Authors.Remove(Author);
        }

        public async Task<IEnumerable<Course>> GetCoursesOfInterestAsync(Guid AuthorId)
        {
            var Author = await context.Authors.Where(c => c.Id == AuthorId).Include(c => c.Courses).FirstOrDefaultAsync();

            return Author.Courses;
        }

        public async Task<Course> GetCourseAsync(Guid id, Guid AuthorId)
        {
            return await context.Courses.Where(p => p.Id == id && p.AuthorId == AuthorId).FirstOrDefaultAsync();
        }

        public async Task AddCourseAsync(Guid AuthorId, Course course)
        {
            var Author = await context.Authors.Where(c => c.Id == AuthorId).Include(c => c.Courses).FirstOrDefaultAsync();

            Author.Courses.Add(course);

            await context.Courses.AddAsync(course);
        }

        public async Task RemoveCourse(Guid AuthorId, Guid id)
        {
            var Author = await context.Authors.Where(c => c.Id == AuthorId).Include(c => c.Courses).FirstOrDefaultAsync();

            var Course = await context.Courses.Where(p => p.Id == id && p.AuthorId == AuthorId).FirstOrDefaultAsync();

            Author.Courses.Remove(Course);

            context.Courses.Remove(Course);
        }

        //public async Task<User> GetUserAsync(string? email, string? password)
        //{
        //    var user = await context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();

        //    return user;
        //}

        public async Task<bool> IsSavedChanges()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}