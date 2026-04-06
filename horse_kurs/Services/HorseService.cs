using horse_kurs.DTOs;
using horse_kurs.Interfaces;
using horse_kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Services
{
    public class HorseService : IHorseService
    {
        private readonly EquestrianClubContext _context;

        public HorseService(EquestrianClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HorseDto>> GetAllHorsesAsync()
        {
            return await _context.Horses
                .Select(h => new HorseDto
                {
                    Id = h.IdHorse,
                    Name = h.Name,
                    Gender = h.Gender,
                    Breed = h.Breed,
                    HealthStatus = h.StateOfHealth,
                    Status = h.Status
                }).ToListAsync();
        }

        public async Task<HorseDto?> GetHorseByIdAsync(int id)
        {
            var h = await _context.Horses.FindAsync(id);
            if (h == null) return null;

            return new HorseDto
            {
                Id = h.IdHorse,
                Name = h.Name,
                Gender = h.Gender,
                Breed = h.Breed,
                HealthStatus = h.StateOfHealth,
                Status = h.Status
            };
        }

        public async Task<HorseDto> CreateHorseAsync(Horse horse)
        {
            _context.Horses.Add(horse);
            await _context.SaveChangesAsync();

            return new HorseDto { Id = horse.IdHorse, Name = horse.Name };
        }
    }
}