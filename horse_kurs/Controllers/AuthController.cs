using Microsoft.AspNetCore.Mvc;
using horse_kurs.DTOs;
using horse_kurs.Services;

namespace horse_kurs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Login) || string.IsNullOrEmpty(loginDto.Password))
                return BadRequest(new { message = "Логин и пароль обязательны" });

            var response = await _authService.Login(loginDto);
            if (response == null)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Login) || string.IsNullOrEmpty(registerDto.Password))
                return BadRequest(new { message = "Логин и пароль обязательны" });

            if (await _authService.UserExists(registerDto.Login))
                return BadRequest(new { message = "Пользователь с таким логином уже существует" });

            var result = await _authService.Register(registerDto);
            if (!result)
                return BadRequest(new { message = "Ошибка при регистрации" });

            return Ok(new { message = "Регистрация успешна" });
        }
    }
}