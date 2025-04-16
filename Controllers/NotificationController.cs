using Microsoft.AspNetCore.Mvc;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        [HttpPost("send")]
        public IActionResult SendNotification() => Ok("Send notification endpoint");

        [HttpGet("history")]
        public IActionResult GetNotificationHistory() => Ok("Get notification history endpoint");
    }
}
