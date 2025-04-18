using Microsoft.AspNetCore.Mvc;
using bloodsociety.DTO;
using bloodsociety.Models;
using bloodsociety.Data;
using Microsoft.EntityFrameworkCore;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly BloodSocietyContext _context;
        public RequestController(BloodSocietyContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto request)
        {
            var user = await _context.Users.FindAsync(request.RequesterId);
            if (user == null) return NotFound("Requester not found");
            var bloodRequest = new BloodRequest
            {
                RequesterId = request.RequesterId,
                BloodType = request.BloodType,
                Location = request.Location,
                Visibility = request.Visibility,
                Status = "Open",
                CreatedAt = DateTime.UtcNow
            };
            _context.BloodRequests.Add(bloodRequest);
            await _context.SaveChangesAsync();
            return Ok(new { bloodRequest.RequestId });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRequests([FromQuery] SearchRequestsDto request)
        {
            var query = _context.BloodRequests.Include(r => r.Requester).AsQueryable();
            if (!string.IsNullOrEmpty(request.BloodType))
                query = query.Where(r => r.BloodType == request.BloodType);
            if (!string.IsNullOrEmpty(request.Location))
                query = query.Where(r => r.Location.Contains(request.Location));
            if (!string.IsNullOrEmpty(request.Status))
                query = query.Where(r => r.Status == request.Status);
            if (!string.IsNullOrEmpty(request.Visibility))
                query = query.Where(r => r.Visibility == request.Visibility);
            var results = await query.OrderByDescending(r => r.CreatedAt)
                .Select(r => new {
                    r.RequestId,
                    r.BloodType,
                    r.Location,
                    r.Status,
                    r.Visibility,
                    r.CreatedAt,
                    Requester = new {
                        r.RequesterId,
                        r.Requester.Name,
                        r.Requester.Phone
                    }
                }).ToListAsync();
            return Ok(results);
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] UpdateRequestDto request)
        {
            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest == null) return NotFound("Request not found");
            if (!string.IsNullOrEmpty(request.BloodType))
                bloodRequest.BloodType = request.BloodType;
            if (!string.IsNullOrEmpty(request.Location))
                bloodRequest.Location = request.Location;
            if (!string.IsNullOrEmpty(request.Visibility))
                bloodRequest.Visibility = request.Visibility;
            if (!string.IsNullOrEmpty(request.Status))
                bloodRequest.Status = request.Status;
            await _context.SaveChangesAsync();
            return Ok("Request updated successfully");
        }

        [HttpDelete("{id}/cancel")]
        public async Task<IActionResult> CancelRequest(int id)
        {
            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest == null) return NotFound("Request not found");
            bloodRequest.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return Ok("Request cancelled successfully");
        }
    }
}
