using horse_kurs.Models;
using horse_kurs.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace horse_kurs.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> Login(LoginDto loginDto);
        Task<bool> Register(RegisterDto registerDto);
        Task<bool> UserExists(string login);
    }

    public class AuthService : IAuthService
    {
        private readonly EquestrianClubContext _context;

        public AuthService(EquestrianClubContext context)
        {
            _context = context;
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            var user = await _context.AppUsers
                .Include(u => u.Client)
                .Include(u => u.Coach)          
                    .ThenInclude(c => c.IdEmployeeNavigation) 
                .FirstOrDefaultAsync(u => u.Login == loginDto.Login && u.Password == loginDto.Password);

            if (user == null)
                return null;

            return new AuthResponseDto
            {
                IdUser = user.IdUser,
                Login = user.Login,
                Role = user.Role,
                IdClient = user.IdClient,
                IdCoach = user.IdCoach,          
                FullName = user.Client != null
                    ? $"{user.Client.Surname} {user.Client.Name} {user.Client.Patronymic}"
                    : (user.Coach?.IdEmployeeNavigation != null
                        ? $"{user.Coach.IdEmployeeNavigation.Surname} {user.Coach.IdEmployeeNavigation.Name} {user.Coach.IdEmployeeNavigation.Patronymic}"
                        : null),
                Token = GenerateToken(user)
            };
        }

        private string GenerateToken(AppUser user)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.IdUser}|{user.Login}|{user.Role}|{DateTime.UtcNow.Ticks}"));
        }

        public async Task<bool> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Login))
                return false;

            var user = new AppUser
            {
                Login = registerDto.Login,
                Password = registerDto.Password,
                Role = "Client",
                IdClient = registerDto.IdClient
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserExists(string login)
        {
            return await _context.AppUsers.AnyAsync(u => u.Login == login);
        }
    }
}