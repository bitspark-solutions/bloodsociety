using Microsoft.AspNetCore.Mvc;
using bloodsociety.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly BloodSocietyContext _context;
        public AdminController(BloodSocietyContext context)
        {
            _context = context;
        }

        // Dashboard analytics: total users, donors, requests, fulfilled requests, reviews, active donors, etc.
        [HttpGet("analytics/dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalDonors = await _context.DonorProfiles.CountAsync();
            var totalRequests = await _context.BloodRequests.CountAsync();
            var fulfilledRequests = await _context.BloodRequests.CountAsync(r => r.Status == "Fulfilled");
            var openRequests = await _context.BloodRequests.CountAsync(r => r.Status == "Open");
            var totalReviews = await _context.Reviews.CountAsync();
            var activeDonors = await _context.DonorProfiles.CountAsync(d => d.ActiveStatus);

            return Ok(new
            {
                totalUsers,
                totalDonors,
                totalRequests,
                fulfilledRequests,
                openRequests,
                totalReviews,
                activeDonors
            });
        }

        // User management: list users, block/unblock, promote/demote
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.OrderByDescending(u => u.CreatedAt)
                .Select(u => new {
                    u.UserId,
                    u.Name,
                    u.Email,
                    u.Phone,
                    u.Role,
                    u.CreatedAt
                }).ToListAsync();
            return Ok(users);
        }

        [HttpPut("users/{userId}/block")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found");
            user.Role = "Blocked";
            await _context.SaveChangesAsync();
            return Ok(new { message = "User blocked" });
        }

        [HttpPut("users/{userId}/unblock")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found");
            if (user.Role == "Blocked") user.Role = "Donor"; // Default to Donor, or restore previous role if tracked
            await _context.SaveChangesAsync();
            return Ok(new { message = "User unblocked" });
        }

        [HttpPut("users/{userId}/promote/{role}")]
        public async Task<IActionResult> PromoteUser(int userId, string role)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found");
            user.Role = role;
            await _context.SaveChangesAsync();
            return Ok(new { message = $"User promoted to {role}" });
        }

        // Analytical: Donor and request trends over time (monthly)
        [HttpGet("analytics/trends")]
        public async Task<IActionResult> GetTrends()
        {
            var now = DateTime.UtcNow;
            var months = Enumerable.Range(0, 6).Select(i => now.AddMonths(-i)).Reverse().ToList();
            var donorTrends = await Task.WhenAll(months.Select(async m => new {
                Month = m.ToString("yyyy-MM"),
                Count = await _context.DonorProfiles.CountAsync(d => d.User.CreatedAt.Year == m.Year && d.User.CreatedAt.Month == m.Month)
            }));
            var requestTrends = await Task.WhenAll(months.Select(async m => new {
                Month = m.ToString("yyyy-MM"),
                Count = await _context.BloodRequests.CountAsync(r => r.CreatedAt.Year == m.Year && r.CreatedAt.Month == m.Month)
            }));
            return Ok(new { donorTrends, requestTrends });
        }

        // Analytical: Most active regions/cities (by request count)
        [HttpGet("analytics/regions")]
        public async Task<IActionResult> GetActiveRegions()
        {
            var regionStats = await _context.BloodRequests
                .GroupBy(r => r.Location)
                .Select(g => new { Location = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(10)
                .ToListAsync();
            return Ok(regionStats);
        }

        // Analytical: Blood type statistics (donors and requests)
        [HttpGet("analytics/bloodtypes")]
        public async Task<IActionResult> GetBloodTypeStats()
        {
            var donorStats = await _context.DonorProfiles
                .GroupBy(d => d.BloodType)
                .Select(g => new { BloodType = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToListAsync();
            var requestStats = await _context.BloodRequests
                .GroupBy(r => r.BloodType)
                .Select(g => new { BloodType = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToListAsync();
            return Ok(new { donorStats, requestStats });
        }

        // System health/status: count of notifications, reviews, chat messages
        [HttpGet("analytics/system-status")]
        public async Task<IActionResult> GetSystemStatus()
        {
            var notifications = await _context.Notifications.CountAsync();
            var reviews = await _context.Reviews.CountAsync();
            var chatMessages = await _context.ChatMessages.CountAsync();
            return Ok(new { notifications, reviews, chatMessages });
        }

        // Recent activity logs (last 20 requests, reviews, users)
        [HttpGet("activity/recent")]
        public async Task<IActionResult> GetRecentActivity()
        {
            var recentRequests = await _context.BloodRequests.OrderByDescending(r => r.CreatedAt).Take(20).Select(r => new {
                r.RequestId, r.BloodType, r.Location, r.Status, r.CreatedAt
            }).ToListAsync();
            var recentReviews = await _context.Reviews.OrderByDescending(r => r.CreatedAt).Take(20).Select(r => new {
                r.ReviewId, r.DonorId, r.ReceiverId, r.Rating, r.CreatedAt
            }).ToListAsync();
            var recentUsers = await _context.Users.OrderByDescending(u => u.CreatedAt).Take(20).Select(u => new {
                u.UserId, u.Name, u.Role, u.CreatedAt
            }).ToListAsync();
            return Ok(new { recentRequests, recentReviews, recentUsers });
        }
    }
}
