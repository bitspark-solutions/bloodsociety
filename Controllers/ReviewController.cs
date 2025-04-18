using Microsoft.AspNetCore.Mvc;
using bloodsociety.DTO;
using bloodsociety.Models;
using bloodsociety.Data;
using Microsoft.EntityFrameworkCore;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly BloodSocietyContext _context;
        public ReviewController(BloodSocietyContext context)
        {
            _context = context;
        }

        [HttpPost("give-review")]
        public async Task<IActionResult> GiveReview([FromBody] GiveReviewRequest request)
        {
            if (request.Rating < 1 || request.Rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            var donor = await _context.Users.FindAsync(request.DonorId);
            var receiver = await _context.Users.FindAsync(request.ReceiverId);
            if (donor == null || receiver == null)
                return NotFound("Donor or receiver not found");

            var review = new Review
            {
                DonorId = request.DonorId,
                ReceiverId = request.ReceiverId,
                Rating = request.Rating,
                Comments = request.Comments,
                CreatedAt = DateTime.UtcNow
            };
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok("Review added successfully");
        }

        [HttpGet("donor/{id}")]
        public async Task<IActionResult> GetDonorReviews(int id)
        {
            var reviews = await _context.Reviews
                .Where(r => r.DonorId == id)
                .Include(r => r.Receiver)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new {
                    r.ReviewId,
                    r.Rating,
                    r.Comments,
                    r.CreatedAt,
                    Receiver = new {
                        r.ReceiverId,
                        r.Receiver.Name
                    }
                })
                .ToListAsync();
            return Ok(reviews);
        }
    }
}
