using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieBookingDbContext _context;

        public MoviesController(MovieBookingDbContext context)
        {
            _context = context;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies([FromQuery] string lang, [FromQuery] string genre)
        {
            IQueryable<Movie> query = _context.Movies;
            if (lang == "*")
            {

            }
            else if (!string.IsNullOrEmpty(lang))
            {
                query = query.Where(m => m.Lang == lang);
            }

            if (genre == "*")
            {

            }
            else if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(m => m.Genre == genre);
            }

            return await query.ToListAsync();
        }

        // GET: api/movies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // POST: api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.UtcNow;
            movie.UpdatedAt = DateTime.UtcNow;

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // DELETE: api/movies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(long id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/movies/{id}/reviews
        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetMovieReviews(long id)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MovieId == id)
                .ToListAsync();

            return reviews;
        }

        // GET: api/movies/{id}/shows
        [HttpGet("{id}/shows")]
        public async Task<ActionResult<IEnumerable<Show>>> GetMovieShows(long id)
        {
            var shows = await _context.Shows
                .Where(s => s.MovieId == id)
                .ToListAsync();

            return shows;
        }

        // GET: api/movies/filter?location={city}&language={lang}&genre={genre}
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Movie>>> FilterMovies([FromQuery] string location, [FromQuery] string language, [FromQuery] string genre)
        {
            // Join movies -> shows -> screens -> theatres to filter by location if provided
            var query = _context.Movies.AsQueryable();

            if (!location.Equals("*") || !language.Equals("*") || !genre.Equals("*"))
            {
                // Join with shows and theatres only if location is provided
                if (!location.Equals("*"))
                {
                    query = from m in _context.Movies
                            join s in _context.Shows on m.Id equals s.MovieId
                            join sc in _context.Screens on s.ScreenId equals sc.Id
                            join t in _context.Theatres on sc.TheatreId equals t.Id
                            where t.City.Equals(location)
                            select m;
                }

                if (!language.Equals("*"))
                {
                    query = query.Where(m => m.Lang == language);
                }

                if (!genre.Equals("*"))
                {
                    query = query.Where(m => m.Genre.Contains(genre));
                }
            }

            return await query.ToListAsync();
        }

        // GET: api/movies/toprated
        [HttpGet("toprated")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetTopRatedMovies()
        {
            var topMovies = await _context.Movies
                .Where(m => m.Rating != null)
                .OrderByDescending(m => m.Rating)
                .Take(5)
                .ToListAsync();

            return topMovies;
        }

        // GET: api/movies/released?start=YYYY-MM-DD&end=YYYY-MM-DD
        [HttpGet("released")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByReleaseDateRange([FromQuery] string start, [FromQuery] string end)
        {
            if (!DateOnly.TryParse(start, out var startDate) || !DateOnly.TryParse(end, out var endDate))
            {
                return BadRequest("Invalid date format. Use YYYY-MM-DD.");
            }

            var movies = await _context.Movies
                .Where(m => m.ReleaseDate != null && m.ReleaseDate >= startDate && m.ReleaseDate <= endDate)
                .ToListAsync();

            return movies;
        }

        // GET: api/movies/moviesCount
        [HttpGet("moviesCount")]
        public async Task<ActionResult<int>> GetMoviesCount()
        {
            var count = await _context.Movies.CountAsync();
            return count;
        }
    }
}