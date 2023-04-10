using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository repository;
        private readonly IMapper mapper;

        public CoursesController(ICourseLibraryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(Guid AuthorId)
        {
            var entity = await repository.GetCoursesOfInterestAsync(AuthorId);

            return Ok(mapper.Map<IEnumerable<CourseDto>>(entity));
            //return Ok(entity.Select(e => mapper.Map<IEnumerable< CourseDto>>(e)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetOneCourse(Guid AuthorId, Guid id)
        {
            var Author = await repository.GetAuthorAsync(AuthorId);

            if (Author == null)
            {
                return NotFound();
            }

            var entity = await repository.GetCourseAsync(AuthorId, id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CourseDto>(entity));
        }

        [HttpPost("AddCourse")]
        public async Task<ActionResult> AddCourse(Guid AuthorId, CourseToAdd body)
        {
            var Author = await repository.GetAuthorAsync(AuthorId);

            if (Author == null)
            {
                return NotFound();
            }

            var entityCourse = mapper.Map<Course>(body);

            await repository.AddCourseAsync(AuthorId, entityCourse);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("UpdateCourse")]
        public async Task<ActionResult> UpdateCourse(Guid AuthorId, Guid id, CourseToAdd Course)
        {
            var Author = await repository.GetAuthorAsync(AuthorId);

            if (Author == null)
            {
                return NotFound();
            }

            var entityCourse = await repository.GetCourseAsync(id, AuthorId);

            if (entityCourse == null)
            {
                return NotFound();
            }

            mapper.Map(Course, entityCourse);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch("UpdateCoursePartially")]
        public async Task<ActionResult> UpdateCoursePartially(Guid AuthorId, Guid id, JsonPatchDocument<CourseToAdd> patch)
        {
            var Author = await repository.GetAuthorAsync(AuthorId);

            if (Author == null)
            {
                return NotFound();
            }

            var entityCourse = await repository.GetCourseAsync(id, AuthorId);

            if (entityCourse == null)
            {
                return NotFound();
            }

            var CourseToAdd = new CourseToAdd();

            mapper.Map(entityCourse, CourseToAdd);

            patch.ApplyTo(CourseToAdd, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            mapper.Map(CourseToAdd, entityCourse);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("DeleteCourse")]
        public async Task<ActionResult> DeleteCourse(Guid AuthorId, Guid id)
        {
            await repository.RemoveCourse(AuthorId, id);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}