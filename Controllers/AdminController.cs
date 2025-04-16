using Microsoft.AspNetCore.Mvc;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet("analytics/dashboard")]
        public IActionResult GetDashboard() => Ok("Get analytics dashboard endpoint");
    }
}
