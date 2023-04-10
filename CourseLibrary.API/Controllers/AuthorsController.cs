using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository repository;
        private readonly IMapper mapper;
        private const int MAX_SIZE = 10;

        public AuthorsController(ICourseLibraryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all the Authors
        /// </summary>
        /// <param name="name">Author name</param>
        /// <param name="search">a string to search in name or description of the Author</param>
        /// <param name="pageSize">specify the number of elements in a page</param>
        /// <param name="pageNumber">choose the page number</param>
        /// <returns></returns>
        /// <response code="200">List of Authors</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors(string? name, string? search, [Required] int pageSize, [Required] int pageNumber)
        {
            if (pageSize > MAX_SIZE)
            {
                pageSize = MAX_SIZE;
            }
            var entity = await repository.GetAuthorsAsync(name, search, pageSize, pageNumber);

            // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paging));
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(entity));
            //return Ok(entity.Select(e => mapper.Map<AuthorDto>(e)));
        }

        /// <summary>
        /// returns specified Author by its id
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        ///<response code="404">Author NotFound</response>
        /// <response code="200">Returns with the Author</response>
        [HttpGet("{AuthorId}")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        public async Task<ActionResult<AuthorDto>> GetOneAuthor(Guid AuthorId, int version)
        {
            var entity = await repository.GetAuthorAsync(AuthorId);

            if (entity == null)
            {
                return NotFound();
            }

            //var Courses = entity.CourseOfInterests.Select(p => mapper.Map<AuthorDto>(p));

            return Ok(mapper.Map<AuthorDto>(entity));
        }

        /// <summary>
        /// Adding Author to the list of Authors
        /// </summary>
        /// <remarks>
        /// {
        ///     "name": "",
        ///     "Description": ""
        /// }
        /// </remarks>
        /// <param name="Author"></param>
        /// <returns></returns>
        /// <response code="500">Adding Failed</response>
        /// <response code="201">Adding Succeed</response>
        [HttpPost("AddAuthor")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> AddAuthor(AuthorToUpdate body)
        {
            var entity = mapper.Map<Author>(body);

            await repository.AddAuthorAsync(entity);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        /// Updating the whole Author props
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <param name="Author"></param>
        /// <returns></returns>
        /// <response code="404">Author NotFound</response>
        /// <response code="201">Update Succeed</response>
        [HttpPut("UpdateAuthor")]
        public async Task<ActionResult> UpdateAuthor(Guid AuthorId, AuthorToUpdate Author)
        {
            var entityAuthor = await repository.GetAuthorAsync(AuthorId);

            if (entityAuthor == null)
            {
                return NotFound();
            }

            mapper.Map(Author, entityAuthor);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        /// updating all or single prop in Author
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        /// <response code="500">Update Failed</response>
        /// <response code="404">Author not Found</response>
        /// <response code="201">Update Succeed</response>
        [HttpPatch("UpdateAuthorPartially")]
        public async Task<ActionResult> UpdateAuthorPartially(Guid AuthorId, JsonPatchDocument<AuthorToUpdate> patch)
        {
            var Author = await repository.GetAuthorAsync(AuthorId);

            if (Author == null)
            {
                return NotFound();
            }

            var AuthorToUpdate = new AuthorToUpdate();

            mapper.Map(Author, AuthorToUpdate);

            patch.ApplyTo(AuthorToUpdate, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(AuthorToUpdate, Author);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        /// Deleteing a Author from the list of Authors
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <returns></returns>
        /// <response code="500">Remove Failed</response>
        /// <response code="201">Remove Succeed</response>
        [HttpDelete("DeleteAuthor")]
        public async Task<ActionResult> DeleteAuthor(Guid AuthorId)
        {
            await repository.RemoveAuthor(AuthorId);

            if (!await repository.IsSavedChanges())
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}