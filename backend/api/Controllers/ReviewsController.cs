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
    public class ReviewsController : ControllerBase
    {
        private readonly MovieBookingDbContext _context;

        public ReviewsController(MovieBookingDbContext context)
        {
            _context = context;
        }

        // GET: api/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews([FromQuery] long? movieId, [FromQuery] long? userId)
        {
            IQueryable<Review> query = _context.Reviews;

            if (movieId.HasValue)
            {
                query = query.Where(r => r.MovieId == movieId.Value);
            }

            if (userId.HasValue)
            {
                query = query.Where(r => r.UserId == userId.Value);
            }

            return await query.ToListAsync();
        }

        // GET: api/reviews/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(long id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        // DELETE: api/reviews/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(long id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/reviews/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchReview(long id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            review.UpdatedAt = DateTime.UtcNow;
            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // GET: api/reviews/top?movieId={movieId}
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<Review>>> GetTopReviews([FromQuery] long movieId)
        {
            // Assuming "top" means highest rated. Adjust logic as needed.
            var topReviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.Rating)
                .Take(5)
                .ToListAsync();

            return topReviews;
        }

        // GET: api/reviews/averageRating?movieId={movieId}
        [HttpGet("averageRating")]
        public async Task<ActionResult<double>> GetAverageRating([FromQuery] long movieId)
        {
            var avgRating = await _context.Reviews
                .Where(r => r.MovieId == movieId && r.Rating != null)
                .AverageAsync(r => (double)r.Rating);

            return avgRating;
        }

        private bool ReviewExists(long id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}