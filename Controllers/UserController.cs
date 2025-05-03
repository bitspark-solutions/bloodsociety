using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Added this line
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bloodsociety.Data;
using bloodsociety.Models;
using bloodsociety.DTO;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly BloodSocietyContext _context;
        public UserController(BloodSocietyContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("User not found");
            return Ok(new
            {
                user.UserId,
                user.Name,
                user.Email,
                user.Phone,
                user.Role,
                user.CreatedAt
            });
        }

        [HttpPut("profile/update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest req)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("User not found");
            user.Name = req.Name ?? user.Name;
            user.Phone = req.Phone ?? user.Phone;
            user.Role = req.Role ?? user.Role;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpGet("roles")]
        [AllowAnonymous]
        public IActionResult GetRoles() => Ok(new[] { "Donor", "Receiver", "Doctor" });
    }

}
