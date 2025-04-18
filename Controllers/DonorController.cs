using Microsoft.AspNetCore.Mvc;
using bloodsociety.DTO;
using bloodsociety.Models;
using bloodsociety.Data;
using Microsoft.EntityFrameworkCore;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly BloodSocietyContext _context;
        public DonorController(BloodSocietyContext context)
        {
            _context = context;
        }

        [HttpPut("set-active")]
        public async Task<IActionResult> SetActive([FromBody] SetActiveRequest request)
        {
            var donor = await _context.DonorProfiles.FirstOrDefaultAsync(d => d.DonorId == request.DonorId);
            if (donor == null) return NotFound("Donor not found");
            donor.ActiveStatus = request.ActiveStatus;
            await _context.SaveChangesAsync();
            return Ok("Donor active status updated");
        }

        [HttpPut("last-donated")]
        public async Task<IActionResult> UpdateLastDonated([FromBody] UpdateLastDonatedRequest request)
        {
            var donor = await _context.DonorProfiles.FirstOrDefaultAsync(d => d.DonorId == request.DonorId);
            if (donor == null) return NotFound("Donor not found");
            donor.LastDonationDate = request.LastDonationDate;
            await _context.SaveChangesAsync();
            return Ok("Last donation date updated");
        }

        [HttpPost("upload-doc")]
        public async Task<IActionResult> UploadDoc([FromBody] UploadDocRequest request)
        {
            var donor = await _context.DonorProfiles.FirstOrDefaultAsync(d => d.DonorId == request.DonorId);
            if (donor == null) return NotFound("Donor not found");
            // For simplicity, store file as base64 string (in real app, use blob storage or file system)
            donor.HealthCertificate = Convert.ToBase64String(request.FileContent);
            await _context.SaveChangesAsync();
            return Ok("Health certificate uploaded");
        }

        [HttpPut("blood-type")]
        public async Task<IActionResult> UpdateBloodType([FromBody] UpdateBloodTypeRequest request)
        {
            var donor = await _context.DonorProfiles.FirstOrDefaultAsync(d => d.DonorId == request.DonorId);
            if (donor == null) return NotFound("Donor not found");
            donor.BloodType = request.BloodType;
            await _context.SaveChangesAsync();
            return Ok("Blood type updated");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchDonors([FromQuery] SearchDonorsRequest request)
        {
            var query = _context.DonorProfiles.Include(d => d.User).AsQueryable();
            if (!string.IsNullOrEmpty(request.BloodType))
                query = query.Where(d => d.BloodType == request.BloodType);
            if (request.ActiveStatus.HasValue)
                query = query.Where(d => d.ActiveStatus == request.ActiveStatus);
            if (request.LastDonatedAfter.HasValue)
                query = query.Where(d => d.LastDonationDate >= request.LastDonatedAfter);
            if (request.LastDonatedBefore.HasValue)
                query = query.Where(d => d.LastDonationDate <= request.LastDonatedBefore);
            // Location filter can be added if location is modeled
            var donors = await query.Select(d => new {
                d.DonorId,
                d.BloodType,
                d.ActiveStatus,
                d.LastDonationDate,
                d.User.Name,
                d.User.Email,
                d.User.Phone
            }).ToListAsync();
            return Ok(donors);
        }
    }
}
