using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StallsController : ControllerBase
    {
        private readonly EquestrianClubContext _context;

        public StallsController(EquestrianClubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stall>>> GetStalls()
        {
            return await _context.Stalls.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stall>> GetStall(int id)
        {
            var stall = await _context.Stalls.FindAsync(id);

            if (stall == null)
            {
                return NotFound();
            }

            return stall;
        }

        [HttpPost]
        public async Task<ActionResult<Stall>> PostStall(Stall stall)
        {
            _context.Stalls.Add(stall);
            await _context.SaveChangesAsync();

            return stall;
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStallStatus(int id, string status)
        {
            var stall = await _context.Stalls.FindAsync(id);
            if (stall == null)
            {
                return NotFound();
            }

            stall.Status = status;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}