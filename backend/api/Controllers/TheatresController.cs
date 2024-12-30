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
    public class TheatresController : ControllerBase
    {
        private readonly MovieBookingDbContext _context;

        public TheatresController(MovieBookingDbContext context)
        {
            _context = context;
        }

        // GET: api/theatres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theatre>>> GetTheatres()
        {
            return await _context.Theatres.ToListAsync();
        }

        // GET: api/theatres/filter?location={location}
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Theatre>>> GetTheatresByLocation([FromQuery] string location)
        {
            var theatres = await _context.Theatres
                .Where(t => t.City == location || t.State == location || t.Address.Contains(location))
                .ToListAsync();

            return theatres;
        }

        // GET: api/theatres/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Theatre>> GetTheatre(long id)
        {
            var theatre = await _context.Theatres.FindAsync(id);

            if (theatre == null)
            {
                return NotFound();
            }

            return theatre;
        }

        // POST: api/theatres
        [HttpPost]
        public async Task<ActionResult<Theatre>> PostTheatre(Theatre theatre)
        {
            theatre.CreatedAt = DateTime.UtcNow;
            theatre.UpdatedAt = DateTime.UtcNow;

            _context.Theatres.Add(theatre);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTheatre), new { id = theatre.Id }, theatre);
        }

        // PUT: api/theatres/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheatre(long id, Theatre theatre)
        {
            if (id != theatre.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            theatre.UpdatedAt = DateTime.UtcNow;
            _context.Entry(theatre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TheatreExists(id))
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

        // DELETE: api/theatres/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheatre(long id)
        {
            var theatre = await _context.Theatres.FindAsync(id);
            if (theatre == null)
            {
                return NotFound();
            }

            _context.Theatres.Remove(theatre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/theatres/Locations
        [HttpGet("Locations")]
        public async Task<ActionResult<IEnumerable<string>>> GetLocations()
        {
            var locations = await _context.Theatres
                .Select(t => t.City)
                .Distinct()
                .ToListAsync();

            return locations;
        }

        // GET: api/theatres/{id}/Shows
        [HttpGet("{id}/Shows")]
        public async Task<ActionResult<IEnumerable<Show>>> GetShowsOfTheatre(long id)
        {

            var shows = await _context.Shows
                .Where(s => _context.Screens
                    .Any(screen => screen.Id == s.ScreenId && screen.TheatreId == id))
                .ToListAsync();

            return shows;
        }

        // GET: api/theatres/theatresCount
        [HttpGet("theatresCount")]
        public async Task<ActionResult<int>> GetTheatresCount()
        {
            var count = await _context.Theatres.CountAsync();
            return count;
        }

        // GET: api/theatres/Cities
        [HttpGet("Cities")]
        public async Task<ActionResult<IEnumerable<string>>> GetCities()
        {
            var cities = await _context.Theatres
                .Select(t => t.City)
                .Distinct()
                .ToListAsync();

            return cities;
        }

        // GET: api/theatres/States
        [HttpGet("States")]
        public async Task<ActionResult<IEnumerable<string>>> GetStates()
        {
            var states = await _context.Theatres
                .Select(t => t.State)
                .Distinct()
                .ToListAsync();

            return states;
        }

        private bool TheatreExists(long id)
        {
            return _context.Theatres.Any(e => e.Id == id);
        }
    }
}