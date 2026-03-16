using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EquestrianClubContext _context;

        public AuthController(EquestrianClubContext context)
        {
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto login)
        {
            try
            {
                // Проверка администратора
                if (login.Login == "admin" && login.Password == "admin")
                {
                    var token = GenerateJwtToken("admin", 0, "Администратор");
                    return Ok(new
                    {
                        Success = true,
                        Role = "admin",
                        Name = "Администратор",
                        Id = 0,
                        Token = token,
                        Permissions = new
                        {
                            CanManageAll = true,
                            CanViewReports = true,
                            CanManageUsers = true,
                            CanManageHorses = true,
                            CanManageSchedule = true
                        }
                    });
                }

                // Проверка тренера
                var coach = await _context.Coaches
                    .Include(c => c.IdEmployeeNavigation)
                    .FirstOrDefaultAsync(c => c.IdEmployeeNavigation != null &&
                        c.IdEmployeeNavigation.Phone == login.Login);

                if (coach != null)
                {
                    // В реальном проекте здесь должна быть проверка хешированного пароля
                    // Сейчас используем простую проверку для демо
                    if (login.Password != "123") // Замените на нормальную проверку пароля
                    {
                        return Unauthorized(new { Success = false, Message = "Неверный пароль" });
                    }

                    var token = GenerateJwtToken("coach", coach.IdCoach, coach.IdEmployeeNavigation?.Name ?? "Тренер");

                    return Ok(new
                    {
                        Success = true,
                        Role = "coach",
                        Name = coach.IdEmployeeNavigation?.Name ?? "Тренер",
                        Surname = coach.IdEmployeeNavigation?.Surname,
                        Patronymic = coach.IdEmployeeNavigation?.Patronymic,
                        Id = coach.IdCoach,
                        EmployeeId = coach.IdEmployee,
                        Qualification = coach.Qualification,
                        Specialization = coach.Specialization,
                        Token = token,
                        Permissions = new
                        {
                            CanViewOwnSchedule = true,
                            CanViewOwnStudents = true,
                            CanViewOwnLessons = true,
                            CanViewHorses = true,
                            CanManageOwnLessons = true
                        }
                    });
                }

                // Проверка клиента
                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Phone == login.Login);

                if (client != null)
                {
                    if (login.Password != "123") // Замените на нормальную проверку пароля
                    {
                        return Unauthorized(new { Success = false, Message = "Неверный пароль" });
                    }

                    var token = GenerateJwtToken("client", client.IdClient, client.Name);

                    // Получаем дополнительную информацию о клиенте
                    var activeMembership = await _context.Memberships
                        .Where(m => m.IdClient == client.IdClient && m.Status == "Активен")
                        .FirstOrDefaultAsync();

                    var upcomingLessons = await _context.Lessons
                        .Include(l => l.IdCoachNavigation)
                            .ThenInclude(c => c.IdEmployeeNavigation)
                        .Include(l => l.IdArenaNavigation)
                        .Where(l => l.IdClient == client.IdClient && l.Date >= DateOnly.FromDateTime(DateTime.Today))
                        .OrderBy(l => l.Date)
                        .Take(5)
                        .Select(l => new
                        {
                            l.IdLesson,
                            Date = l.Date,
                            Type = l.Type,
                            CoachName = l.IdCoachNavigation != null && l.IdCoachNavigation.IdEmployeeNavigation != null
                                ? $"{l.IdCoachNavigation.IdEmployeeNavigation.Surname} {l.IdCoachNavigation.IdEmployeeNavigation.Name}"
                                : "Не назначен",
                            ArenaName = l.IdArenaNavigation != null ? l.IdArenaNavigation.Name : "Не назначен"
                        })
                        .ToListAsync();

                    return Ok(new
                    {
                        Success = true,
                        Role = "client",
                        Name = client.Name,
                        Surname = client.Surname,
                        Patronymic = client.Patronymic,
                        Id = client.IdClient,
                        Phone = client.Phone,
                        LevelOfTraining = client.LevelOfTraining,
                        Balance = client.Balance,
                        DateOfRegistration = client.DateOfRegistration,
                        Token = token,
                        ActiveMembership = activeMembership != null ? new
                        {
                            activeMembership.IdMembership,
                            activeMembership.Type,
                            activeMembership.LessonsTotal,
                            activeMembership.ValidFrom,
                            activeMembership.ValidUntil,
                            activeMembership.Price,
                            activeMembership.Status
                        } : null,
                        UpcomingLessons = upcomingLessons,
                        Permissions = new
                        {
                            CanViewOwnProfile = true,
                            CanViewOwnLessons = true,
                            CanViewOwnMemberships = true,
                            CanViewOwnPayments = true,
                            CanBookLessons = true,
                            CanViewHorses = true
                        }
                    });
                }

                return Unauthorized(new { Success = false, Message = "Пользователь не найден" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Ошибка сервера", Error = ex.Message });
            }
        }

        // GET: api/auth/profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<object>> GetProfile()
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userIdClaim = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "Не удалось определить пользователя" });
                }

                switch (role)
                {
                    case "admin":
                        return Ok(new
                        {
                            Success = true,
                            Role = "admin",
                            Name = "Администратор",
                            Id = 0
                        });

                    case "coach":
                        var coach = await _context.Coaches
                            .Include(c => c.IdEmployeeNavigation)
                            .FirstOrDefaultAsync(c => c.IdCoach == userId);

                        if (coach == null)
                            return NotFound(new { Success = false, Message = "Тренер не найден" });

                        return Ok(new
                        {
                            Success = true,
                            Role = "coach",
                            Name = coach.IdEmployeeNavigation?.Name ?? "Тренер",
                            Surname = coach.IdEmployeeNavigation?.Surname,
                            Patronymic = coach.IdEmployeeNavigation?.Patronymic,
                            Id = coach.IdCoach,
                            Qualification = coach.Qualification,
                            Specialization = coach.Specialization,
                            Phone = coach.IdEmployeeNavigation?.Phone
                        });

                    case "client":
                        var client = await _context.Clients
                            .FindAsync(userId);

                        if (client == null)
                            return NotFound(new { Success = false, Message = "Клиент не найден" });

                        return Ok(new
                        {
                            Success = true,
                            Role = "client",
                            Name = client.Name,
                            Surname = client.Surname,
                            Patronymic = client.Patronymic,
                            Id = client.IdClient,
                            Phone = client.Phone,
                            LevelOfTraining = client.LevelOfTraining,
                            Balance = client.Balance
                        });

                    default:
                        return Unauthorized(new { Success = false, Message = "Неизвестная роль" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Ошибка сервера", Error = ex.Message });
            }
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // На стороне клиента нужно удалить токен
            return Ok(new { Success = true, Message = "Выход выполнен успешно" });
        }

        // GET: api/auth/check
        [HttpGet("check")]
        [Authorize]
        public IActionResult CheckAuth()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                Success = true,
                IsAuthenticated = true,
                Role = role,
                Name = name
            });
        }

        private string GenerateJwtToken(string role, int userId, string name)
        {
            // В реальном проекте здесь должна быть генерация JWT токена
            // Сейчас возвращаем простой токен для демо
            return $"{role}-token-{userId}-{DateTime.Now.Ticks}";
        }
    }

    public class LoginDto
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterClientDto
    {
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public string Passport { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string House { get; set; } = null!;
        public string? Flat { get; set; }
        public string? LevelOfTraining { get; set; }
    }
}