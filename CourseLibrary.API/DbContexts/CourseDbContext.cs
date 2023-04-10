using CourseLibrary.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CourseLibrary.API.DbContexts
{
    public class CourseDbContext : IdentityUserContext<IdentityUser>
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>().HasData(
                new Author
                {
                    Id = Guid.Parse("db4acbc7-7d58-4a63-a5dc-b05208d7006a"),
                    DateOfBirth = new DateTime(1998, 1, 12),
                    FirstName = "Youssef",
                    LastName = "Ahmed",
                    MainCategory = "Programming"
                },
                new Author
                {
                    Id = Guid.Parse("bb03c19c-a974-41fa-b201-f06fb4995bda"),
                    DateOfBirth = new DateTime(2000, 7, 16),
                    FirstName = "Ali",
                    LastName = "Othman",
                    MainCategory = "Testing"
                }
            );

            builder.Entity<Course>().HasData(
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "C# course",
                    Description = "Learn the fundamentals of c#",
                    AuthorId = Guid.Parse("db4acbc7-7d58-4a63-a5dc-b05208d7006a")
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "ASP.NET CORE",
                    Description = "Learn the fundamentals of ASP.NET",
                    AuthorId = Guid.Parse("bb03c19c-a974-41fa-b201-f06fb4995bda")
                }
            );

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties()
                        .Where(p => p.PropertyType == typeof(DateTimeOffset)
                        || p.PropertyType == typeof(DateTimeOffset?));

                    foreach (var property in properties)
                    {
                        builder.Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}