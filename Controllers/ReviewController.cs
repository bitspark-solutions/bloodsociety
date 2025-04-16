using Microsoft.AspNetCore.Mvc;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        [HttpPost("give-review")]
        public IActionResult GiveReview() => Ok("Add review endpoint");

        [HttpGet("donor/{id}")]
        public IActionResult GetDonorReviews(int id) => Ok($"Get reviews for donor {id} endpoint");
    }
}
