using Microsoft.AspNetCore.Mvc;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateRequest() => Ok("Create donation request endpoint");

        [HttpGet("search")]
        public IActionResult SearchRequests() => Ok("Search donation requests endpoint");

        [HttpPut("{id}/update")]
        public IActionResult UpdateRequest(int id) => Ok($"Update request {id} endpoint");

        [HttpDelete("{id}/cancel")]
        public IActionResult CancelRequest(int id) => Ok($"Cancel request {id} endpoint");
    }
}
