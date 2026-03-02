using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly EquestrianClubContext _context;

        public CompetitionsController(EquestrianClubContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
        {
            return await _context.Competitions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Competition>> GetCompetition(int id)
        {
            var competition = await _context.Competitions.FindAsync(id);

            if (competition == null)
            {
                return NotFound();
            }

            return competition;
        }

        [HttpPost]
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
            _context.Competitions.Add(competition);
            await _context.SaveChangesAsync();

            return competition;
        }
    }
}