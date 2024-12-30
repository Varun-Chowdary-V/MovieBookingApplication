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
    public class BookingsController : ControllerBase
    {
        private readonly MovieBookingDbContext _context;

        public BookingsController(MovieBookingDbContext context)
        {
            _context = context;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings([FromQuery] long? userId, [FromQuery] long? showId)
        {
            IQueryable<Booking> query = _context.Bookings;
            Console.WriteLine(query);

            if (userId.HasValue)
            {
                query = query.Where(b => b.UserId == userId.Value);
            }

            if (showId.HasValue)
            {
                query = query.Where(b => b.ShowId == showId.Value);
            }

            if (query==null)
            {
                return NotFound();
            }

            return await query.ToListAsync();
        }

        // GET: api/bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(long id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking([FromBody] Booking booking)
        {
            booking.CreatedAt = DateTime.UtcNow;
            booking.UpdatedAt = DateTime.UtcNow;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // DELETE: api/bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(long id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(long id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            booking.UpdatedAt = DateTime.UtcNow;
            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // GET: api/bookings/bookingsCount
        [HttpGet("bookingsCount")]
        public async Task<ActionResult<int>> GetBookingsCount()
        {
            var count = await _context.Bookings.CountAsync();
            return count;
        }

        // GET: api/bookings/top-shows
        [HttpGet("top-shows")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopShows()
        {
            var showCounts = await _context.Bookings
                .GroupBy(b => b.ShowId)
                .Select(g => new { ShowId = g.Key, BookingCount = g.Count() })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToListAsync();

            return showCounts.Cast<object>().ToList();
        }

        //GET: api/bookings/users/{id}
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> getBookingsOfUser([FromQuery] long userId)
        {
            var bookings = await _context.Bookings.Where(booking => booking.UserId == userId).ToListAsync();
            if (bookings == null)
            {
                return NotFound();
            }
            return bookings;
        }

        private bool BookingExists(long id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}