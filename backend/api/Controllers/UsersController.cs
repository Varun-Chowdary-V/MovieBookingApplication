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
    public class UsersController : ControllerBase
    {
        private readonly MovieBookingDbContext _context;

        public UsersController(MovieBookingDbContext context)
        {
            _context = context;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/users/{id}/bookings
        [HttpGet("{id}/Bookings")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetUserBookings(long id)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == id)
                .ToListAsync();

            return bookings;
        }

        // GET: api/users/{id}/reviews
        [HttpGet("{id}/Reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsOfUser(long id)
        {
            var reviews = await _context.Reviews
                .Where(r => r.UserId == id)
                .ToListAsync();

            return reviews;
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.PasswordHashed == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return new { id = user.Id };
        }

        // PATCH: api/users/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserRole(long id, [FromBody] RoleUpdateRequest roleUpdateRequest)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = roleUpdateRequest.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/users/usersCount
        [HttpGet("usersCount")]
        public async Task<ActionResult<int>> GetUsersCount()
        {
            var count = await _context.Users.CountAsync();
            return count;
        }

        // GET: api/users/search?email={email}
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsersByEmail([FromQuery] string email)
        {
            var users = await _context.Users
                .Where(u => u.Email.Contains(email))
                .ToListAsync();

            return users;
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RoleUpdateRequest
        {
            public string Role { get; set; }
        }
    }
}