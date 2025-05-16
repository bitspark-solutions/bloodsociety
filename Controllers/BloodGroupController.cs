using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bloodsociety.Data;
using bloodsociety.Models;
using System;
using System.Threading.Tasks;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodGroupController : ControllerBase
    {
        private readonly BloodSocietyContext _context;

        public BloodGroupController(BloodSocietyContext context)
        {
            _context = context;
        }

        // GET: api/bloodgroup
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bloodGroups = await _context.BloodGroups.ToListAsync();
            return Ok(bloodGroups);
        }

        // POST: api/bloodgroup
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BloodGroup bloodGroup)
        {
            try
            {
                _context.BloodGroups.Add(bloodGroup);
                await _context.SaveChangesAsync();
                return Ok(bloodGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}