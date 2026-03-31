using horse_kurs.DTOs;
using horse_kurs.Models;

namespace horse_kurs.Interfaces
{
    public interface IHorseService
    {
        Task<IEnumerable<HorseDto>> GetAllHorsesAsync();
        Task<HorseDto?> GetHorseByIdAsync(int id);
        Task<HorseDto> CreateHorseAsync(Horse horse);
    }
}