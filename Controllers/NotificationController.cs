using Microsoft.AspNetCore.Mvc;
using bloodsociety.Data;
using bloodsociety.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly BloodSocietyContext _context;
        private readonly KafkaNotificationService _kafkaService;
        private readonly IConfiguration _configuration;
        public NotificationController(BloodSocietyContext context, KafkaNotificationService kafkaService, IConfiguration configuration)
        {
            _context = context;
            _kafkaService = kafkaService;
            _configuration = configuration;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] string message)
        {
            var activeDonors = await _context.DonorProfiles.Include(d => d.User).Where(d => d.ActiveStatus).ToListAsync();
            foreach (var donor in activeDonors)
            {
                var payload = new {
                    UserId = donor.DonorId,
                    Email = donor.User.Email,
                    Name = donor.User.Name,
                    Message = message
                };
                var json = System.Text.Json.JsonSerializer.Serialize(payload);
                await _kafkaService.SendNotificationAsync(json);
            }
            return Ok($"Notification sent to {activeDonors.Count} active donors.");
        }

        [HttpGet("history")]
        public IActionResult GetNotificationHistory() => Ok("Get notification history endpoint");
    }
}
