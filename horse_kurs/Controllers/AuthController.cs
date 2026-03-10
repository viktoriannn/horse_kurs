using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;
using System.Text.Json;

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


            if (login.Login == "admin" && login.Password == "admin")
            {
                return Ok(new
                {
                    Success = true,
                    Role = "admin",
                    Name = "Администратор",
                    Token = "admin-token-" + DateTime.Now.Ticks
                });
            }

            var coach = await _context.Coaches
                .Include(c => c.IdEmployeeNavigation)
                .FirstOrDefaultAsync(c => c.IdEmployeeNavigation != null &&
                    c.IdEmployeeNavigation.Phone == login.Login &&
                    login.Password == "123");

            if (coach != null)
            {
                return Ok(new
                {
                    Success = true,
                    Role = "coach",
                    Name = coach.IdEmployeeNavigation?.Name ?? "Тренер",
                    Id = coach.IdCoach,
                    Token = "coach-token-" + DateTime.Now.Ticks
                });
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Phone == login.Login &&
                    login.Password == "123"); 

            if (client != null)
            {
                return Ok(new
                {
                    Success = true,
                    Role = "client",
                    Name = client.Name,
                    Id = client.IdClient,
                    Token = "client-token-" + DateTime.Now.Ticks
                });
            }

            return Unauthorized(new { Success = false, Message = "Неверный логин или пароль" });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { Success = true, Message = "Выход выполнен" });
        }
    }

    public class LoginDto
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}