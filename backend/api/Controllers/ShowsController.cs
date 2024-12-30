using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly MovieBookingDbContext _context;

        public ShowsController(MovieBookingDbContext context)
        {
            _context = context;
        }

        // GET: api/shows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Show>>> GetAllShows()
        {
            return await _context.Shows.ToListAsync();
        }

        // GET: api/shows/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Show>> GetShowById(long id)
        {
            var show = await _context.Shows.FindAsync(id);

            if (show == null)
            {
                return NotFound();
            }

            return show;
        }

        // GET: api/shows/showsCount
        [HttpGet("showsCount")]
        public async Task<ActionResult<int>> GetShowsCount()
        {
            return await _context.Shows.CountAsync();
        }

        // POST: api/shows
        [HttpPost]
        public async Task<ActionResult<Show>> PostShow(Show show)
        {
            show.CreatedAt = DateTime.UtcNow;
            show.UpdatedAt = DateTime.UtcNow;

            _context.Shows.Add(show);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShowById), new { id = show.Id }, show);
        }

        // DELETE: api/shows/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(long id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }

            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/shows/{id}/bookings
        [HttpGet("{id}/bookings")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsOfShow(long id)
        {
            var bookings = await _context.Bookings
                .Where(b => b.ShowId == id)
                .ToListAsync();

            return bookings;
        }

        // GET: api/shows?movieId={movieId}
        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<Show>>> GetShowsByMovieId([FromQuery] long movieId)
        {
            var shows = await _context.Shows
                .Where(s => s.MovieId == movieId)
                .ToListAsync();

            return shows;
        }

        // GET: api/shows/theatre/{theatreId}
        [HttpGet("theatre/{theatreId}")]
        public async Task<ActionResult<IEnumerable<Show>>> GetShowsByTheatreId(long theatreId)
        {
            var shows = await _context.Shows
                .Where(s => _context.Screens
                    .Any(screen => screen.Id == s.ScreenId && screen.TheatreId == theatreId))
                .ToListAsync();

            return shows;
        }


        // GET: api/shows?screenId={screenId}
        [HttpGet("scree/{screenId}")]
        public async Task<ActionResult<IEnumerable<Show>>> GetShowsByScreenId([FromQuery] long screenId)
        {
            var shows = await _context.Shows
                .Where(s => s.ScreenId == screenId)
                .ToListAsync();

            return shows;
        }

        // PATCH: api/shows/{showId}
        [HttpPatch("{showId}")]
        public async Task<IActionResult> UpdateAvailableSeats(long showId, [FromBody] UpdateShowRequest body)
        {
            var show = await _context.Shows.FindAsync(showId);
            if (show == null)
            {
                return NotFound();
            }

            show.AvailableSeats = body.Seats;
            show.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/shows/search?city={city}&date={date}
        [HttpGet("search/city={city}&date={date}")]
        public async Task<ActionResult<IEnumerable<Show>>> SearchShows([FromQuery] string city, [FromQuery] string date)
        {
            if (!DateOnly.TryParse(date, out var parsedDate))
            {
                return BadRequest("Invalid date format. Use YYYY-MM-DD.");
            }

            var shows = await _context.Shows
                .Where(s => _context.Screens
                    .Any(screen => screen.Id == s.ScreenId && _context.Theatres
                        .Any(theatre=> theatre.Id == screen.TheatreId && theatre.City == city)))
                .ToListAsync();

            return shows;
        }

        // GET: api/shows/dateRange?start={startDate}&end={endDate}
        [HttpGet("dateRange")]
        public async Task<ActionResult<IEnumerable<Show>>> GetShowsByDateRange([FromQuery] string start, [FromQuery] string end)
        {
            if (!DateOnly.TryParse(start, out var startDate) || !DateOnly.TryParse(end, out var endDate))
            {
                return BadRequest("Invalid date format. Use YYYY-MM-DD.");
            }

            var shows = await _context.Shows
                .Where(s => s.ShowDate >= startDate && s.ShowDate <= endDate)
                .ToListAsync();

            return shows;
        }
        public class UpdateShowRequest
        {
            public string? Seats { get; set; }
        }
    }

}