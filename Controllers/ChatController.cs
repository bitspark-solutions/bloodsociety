using Microsoft.AspNetCore.Mvc;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpPost("send-message")]
        public IActionResult SendMessage() => Ok("Send message endpoint");

        [HttpGet("history")]
        public IActionResult GetChatHistory() => Ok("Get chat history endpoint");
    }
}
