using Microsoft.AspNetCore.Mvc;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        [HttpPut("set-active")]
        public IActionResult SetActive() => Ok("Set donor active status endpoint");

        [HttpPut("last-donated")]
        public IActionResult UpdateLastDonated() => Ok("Update last donation date endpoint");

        [HttpPost("upload-doc")]
        public IActionResult UploadDoc() => Ok("Upload health certificate endpoint");

        [HttpPut("blood-type")]
        public IActionResult UpdateBloodType() => Ok("Update blood type endpoint");

        [HttpGet("search")]
        public IActionResult SearchDonors() => Ok("Search donors endpoint");
    }
}
