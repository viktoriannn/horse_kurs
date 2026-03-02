using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorsesController : ControllerBase
    {
        private readonly EquestrianClubContext _context;

        public HorsesController(EquestrianClubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horse>>> GetHorses()
        {
            return await _context.Horses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Horse>> GetHorse(int id)
        {
            var horse = await _context.Horses.FindAsync(id);

            if (horse == null)
            {
                return NotFound();
            }

            return horse;
        }

        [HttpPost]
        public async Task<ActionResult<Horse>> PostHorse(Horse horse)
        {
            _context.Horses.Add(horse);
            await _context.SaveChangesAsync();

            return horse;
        }
    }
}