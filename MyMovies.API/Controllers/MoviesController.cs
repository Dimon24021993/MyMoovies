using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMovies.BLL.BllModels;
using MyMovies.BLL.Interfaces;
using MyMovies.DAL;
using MyMovies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovies.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DataBaseContext db;
        private IMoviesService MoviesService { get; set; }
        public MoviesController(DataBaseContext dbContext, IMoviesService moviesService)
        {
            db = dbContext;
            MoviesService = moviesService;
        }

        [HttpPost]
        public async Task<ActionResult<Pagination>> GetMoviesPagination(Pagination pagination)
        {
            try
            {
                pagination.Entities = await MoviesService.GetMovies(pagination).ToListAsync();
                pagination.Pages = Convert.ToInt32(Math.Ceiling(MoviesService.TotalCount() / (pagination.Size * 1.0)));
                return Ok(pagination);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public IEnumerable<Comment> GetComents()
        {
            return db.Comments;
        }

        [HttpGet]
        public IEnumerable<Description> GetDescriptions()
        {
            return db.Descriptions;
        }

        [HttpGet]
        public IEnumerable<Job> GetJobs()
        {
            return db.Jobs;
        }

        [HttpGet]
        public IEnumerable<Person> GetPersons()
        {
            return db.Persons;
        }

        [HttpGet]
        public IEnumerable<Movie> GetMovies()
        {
            return db.Movies;
        }

        [HttpGet]
        public IEnumerable<Tag> GetTags()
        {
            return db.Tags;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return db.Users;
        }

        [HttpGet]
        public async Task<IEnumerable<Description>> GetMovieDescriptions(Guid movieId)
        {
            return await MoviesService.GetMovieDescriptions(movieId);
        }

        [HttpGet]
        public async Task<ActionResult<Movie>> GetMovie(Guid movieId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await MoviesService.GetMovieAsync(movieId);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie([FromRoute] Guid id, [FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.Id)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            await db.SaveChangesAsync();

            return Ok(movie);
        }

        private bool MovieExists(Guid id)
        {
            return db.Movies.Any(e => e.Id == id);
        }
    }
}