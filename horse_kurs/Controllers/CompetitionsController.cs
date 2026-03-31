using horse_kurs.DTOs;
using horse_kurs.Interfaces;
using horse_kurs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly EquestrianClubContext _context;
        private readonly ICompetitionService _competitionService;

        public CompetitionsController(EquestrianClubContext context, ICompetitionService competitionService)
        {
            _context = context;
            _competitionService = competitionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
        {
            return await _context.Competitions
                .OrderByDescending(c => c.Date)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCompetitionDetails(int id)
        {
            var competition = await _context.Competitions
                .Include(c => c.Participations)
                    .ThenInclude(p => p.IdClientNavigation)
                .Include(c => c.Participations)
                    .ThenInclude(p => p.IdHorseNavigation)
                .FirstOrDefaultAsync(c => c.IdCompetition == id);

            if (competition == null) return NotFound();

            return Ok(new
            {
                competition.Name,
                competition.Date,
                Participants = competition.Participations.Select(p => new
                {
                    Rider = $"{p.IdClientNavigation.Surname} {p.IdClientNavigation.Name}",
                    Horse = p.IdHorseNavigation.Name,
                    Place = p.ResultPlace,
                    p.Score
                })
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterParticipationDto dto)
        {
            var result = await _competitionService.RegisterRiderAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}