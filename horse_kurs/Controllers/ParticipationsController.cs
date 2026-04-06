using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationsController : ControllerBase
    {
        private readonly EquestrianClubContext _context;

        public ParticipationsController(EquestrianClubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participation>>> GetParticipations()
        {
            return await _context.Participations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Participation>> GetParticipation(int id)
        {
            var participation = await _context.Participations.FindAsync(id);

            if (participation == null)
            {
                return NotFound();
            }

            return participation;
        }

        [HttpGet("competition/{competitionId}")]
        public async Task<ActionResult<IEnumerable<Participation>>> GetByCompetition(int competitionId)
        {
            return await _context.Participations
                .Where(p => p.IdCompetition == competitionId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Participation>> PostParticipation(Participation participation)
        {
            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return participation;
        }
    }
}