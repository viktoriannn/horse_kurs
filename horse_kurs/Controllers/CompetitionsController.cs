using Microsoft.AspNetCore.Mvc;
using horse_kurs.DTOs;
using horse_kurs.Interfaces;

namespace horse_kurs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitionsController : ControllerBase
    {
        private readonly ICompetitionService _competitionService;

        public CompetitionsController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var competitions = await _competitionService.GetAllCompetitionsAsync();
            return Ok(competitions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var competition = await _competitionService.GetCompetitionByIdAsync(id);
            if (competition == null)
                return NotFound();
            return Ok(competition);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterParticipationDto dto)
        {
            var result = await _competitionService.RegisterParticipationAsync(dto);
            if (!result)
                return BadRequest(new { message = "Не удалось зарегистрироваться на соревнование" });

            return Ok(new { message = "Вы успешно зарегистрированы на соревнование" });
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClient(int clientId)
        {
            var participations = await _competitionService.GetParticipationsByClientAsync(clientId);
            return Ok(participations);
        }

        [HttpGet("competition/{competitionId}/participants")]
        public async Task<IActionResult> GetParticipants(int competitionId)
        {
            var participants = await _competitionService.GetParticipationsByCompetitionAsync(competitionId);
            return Ok(participants);
        }
    }
}