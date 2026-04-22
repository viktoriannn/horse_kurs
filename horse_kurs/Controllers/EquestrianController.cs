using Microsoft.AspNetCore.Mvc;
using horse_kurs.DTOs;
using horse_kurs.Services;
using horse_kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquestrianController : ControllerBase
    {
        private readonly IEquestrianService _service;
        private readonly EquestrianClubContext _context;

        public EquestrianController(IEquestrianService service, EquestrianClubContext context)
        {
            _service = service;
            _context = context;
        }


        [HttpGet("admin/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsers();
            return Ok(users);
        }

        [HttpPut("admin/user/{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] string newRole)
        {
            var result = await _service.UpdateUserRole(userId, newRole);
            if (!result)
                return NotFound(new { message = "Пользователь не найден" });
            return Ok(new { message = "Роль обновлена" });
        }

        [HttpGet("admin/users/report")]
        public async Task<IActionResult> GetUsersReport()
        {
            var report = await _service.GetAllUsersReport();
            return File(report, "text/plain", $"users_report_{DateTime.Now:yyyyMMdd}.txt");
        }


        [HttpGet("admin/export/full")]
        public async Task<IActionResult> ExportFullReport()
        {
            try
            {
                var excelService = new ExcelExportService(_context);
                var fileBytes = await excelService.GetFullReportExcelAsync();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"full_report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Ошибка экспорта: {ex.Message}" });
            }
        }


        [HttpPost("admin/employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            try
            {
                var employee = await _service.CreateEmployee(dto);
                return Ok(new { message = "Сотрудник добавлен", id = employee.IdEmployee });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Ошибка: {ex.Message}" });
            }
        }

        [HttpPost("admin/client")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientDto dto)
        {
            try
            {
                var client = await _service.CreateClient(dto);
                return Ok(new { message = "Клиент добавлен", id = client.IdClient });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Ошибка: {ex.Message}" });
            }
        }

        [HttpPost("admin/horse")]
        public async Task<IActionResult> CreateHorse([FromBody] CreateHorseDto dto)
        {
            try
            {
                var horse = await _service.CreateHorse(dto);
                return Ok(new { message = "Лошадь добавлена", id = horse.IdHorse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Ошибка: {ex.Message}" });
            }
        }


        [HttpGet("coach/{coachId}/schedule")]
        public async Task<IActionResult> GetCoachSchedule(int coachId)
        {
            var schedule = await _service.GetCoachSchedule(coachId);
            return Ok(schedule);
        }


        [HttpGet("client/{clientId}/profile")]
        public async Task<IActionResult> GetClientProfile(int clientId)
        {
            var profile = await _service.GetClientProfile(clientId);
            return Ok(profile);
        }

        [HttpGet("competitions")]
        public async Task<IActionResult> GetAvailableCompetitions()
        {
            var competitions = await _service.GetAvailableCompetitions();
            return Ok(competitions);
        }

        [HttpPost("lesson/register")]
        public async Task<IActionResult> RegisterForLesson([FromBody] RegisterLessonDto dto)
        {
            var result = await _service.RegisterForLesson(dto);
            if (!result)
                return BadRequest(new { message = "Недостаточно средств или ошибка записи" });
            return Ok(new { message = "Вы успешно записаны на занятие" });
        }

        [HttpPost("competition/register")]
        public async Task<IActionResult> RegisterForCompetition([FromBody] RegisterCompetitionDto dto)
        {
            var result = await _service.RegisterForCompetition(dto);
            if (!result)
                return BadRequest(new { message = "Ошибка при записи на соревнование" });
            return Ok(new { message = "Вы успешно записаны на соревнование" });
        }

        [HttpGet("client/{clientId}/schedule/print")]
        public async Task<IActionResult> PrintSchedule(int clientId)
        {
            var pdfBytes = await _service.PrintSchedule(clientId);
            return File(pdfBytes, "text/plain", $"schedule_{clientId}.txt");
        }


        [HttpGet("booking/data")]
        public async Task<IActionResult> GetBookingData()
        {
            var data = await _service.GetBookingDataAsync();
            return Ok(data);
        }

        [HttpGet("booking/horses")]
        public async Task<IActionResult> GetAvailableHorses([FromQuery] DateTime date, [FromQuery] string lessonType)
        {
            var horses = await _service.GetAvailableHorsesAsync(date, lessonType);
            return Ok(horses);
        }

        [HttpPost("booking/create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var result = await _service.CreateBookingAsync(dto);
            if (!result)
                return BadRequest(new { message = "Ошибка записи. Проверьте баланс или свободное время тренера" });

            return Ok(new { message = "Занятие успешно запланировано!" });
        }
        [HttpGet("client/{clientId}/schedule")]
        public async Task<IActionResult> GetClientSchedule(int clientId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var schedule = await _service.GetClientSchedule(clientId, startDate, endDate);
            return Ok(schedule);
        }
    }

}