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
    public class ScreensController : ControllerBase
    {
        private readonly MovieBookingDbContext _context;

        public ScreensController(MovieBookingDbContext context)
        {
            _context = context;
        }

        // GET: api/screens/theatres/{theatreId}/screens
        [HttpGet("{theatreId}/screens")]
        public async Task<ActionResult<IEnumerable<Screen>>> GetScreensByTheatreId(long theatreId)
        {
            var screens = await _context.Screens
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();

            return screens;
        }

        // POST: api/screens
        [HttpPost]
        public async Task<ActionResult<Screen>> AddScreenToTheatre(Screen screen)
        {
            _context.Screens.Add(screen);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetScreenById), new { screenId = screen.Id }, screen);
        }

        // GET: api/screens/{screenId}
        [HttpGet("{screenId}")]
        public async Task<ActionResult<Screen>> GetScreenById(long screenId)
        {
            var screen = await _context.Screens.FindAsync(screenId);

            if (screen == null)
            {
                return NotFound();
            }

            return screen;
        }

        // GET: api/screens/{screenId}/theatre
        [HttpGet("{screenId}/theatre")]
        public async Task<ActionResult<Theatre>> getTheatreBySceenId(long screenId)
        {
            var screen = await _context.Screens.FindAsync(screenId);
            var theatre = await _context.Theatres.FindAsync(screen?.TheatreId);
            if (theatre == null)
            {
                return NotFound();
            }
            return theatre;
        }

        // PATCH: api/screens/{screenId}
        [HttpPatch("screens/{screenId}")]
        public async Task<IActionResult> UpdateScreen(long screenId, [FromBody] Screen updatedScreen)
        {
            var screen = await _context.Screens.FindAsync(screenId);
            if (screen == null)
            {
                return NotFound();
            }

            screen.ScreenName = updatedScreen.ScreenName ?? screen.ScreenName;
            screen.Capacity = updatedScreen.Capacity!=-1 ? updatedScreen.Capacity: screen.Capacity;
            screen.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/screens/{screenId}
        [HttpDelete("{screenId}")]
        public async Task<IActionResult> DeleteScreen(long screenId)
        {
            var screen = await _context.Screens.FindAsync(screenId);
            if (screen == null)
            {
                return NotFound();
            }

            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

public partial class AddScreenRequest
{
    public long TheatreId { get; set; }
    public string ScreenName { get; set; } = null!;
    public int Capacity { get; set; }
}