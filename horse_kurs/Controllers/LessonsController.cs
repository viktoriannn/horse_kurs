using horse_kurs.DTOs;
using horse_kurs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] DateTime date)
        {
            return Ok(await _lessonService.GetScheduleAsync(date));
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] LessonCreateDto dto)
        {
            var result = await _lessonService.BookLessonAsync(dto);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(new { message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _lessonService.CancelLessonAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}