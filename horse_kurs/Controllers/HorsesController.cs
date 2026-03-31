using Microsoft.AspNetCore.Mvc;
using horse_kurs.Models;
using horse_kurs.Interfaces;
using horse_kurs.DTOs;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorsesController : ControllerBase
    {
        private readonly IHorseService _horseService;

        public HorsesController(IHorseService horseService)
        {
            _horseService = horseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorseDto>>> GetHorses()
        {
            var horses = await _horseService.GetAllHorsesAsync();
            return Ok(horses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HorseDto>> GetHorse(int id)
        {
            var horse = await _horseService.GetHorseByIdAsync(id);
            return horse == null ? NotFound() : Ok(horse);
        }

        [HttpPost]
        public async Task<ActionResult<HorseDto>> PostHorse(Horse horse)
        {
            var createdHorse = await _horseService.CreateHorseAsync(horse);
            return CreatedAtAction(nameof(GetHorse), new { id = createdHorse.Id }, createdHorse);
        }
    }
}