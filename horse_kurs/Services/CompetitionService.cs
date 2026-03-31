using horse_kurs.DTOs;
using horse_kurs.Interfaces;
using horse_kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly EquestrianClubContext _context;

        public CompetitionService(EquestrianClubContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> RegisterRiderAsync(RegisterParticipationDto dto)
        {
            var comp = await _context.Competitions.FindAsync(dto.CompetitionId);
            if (comp == null || comp.Date < DateOnly.FromDateTime(DateTime.Now))
                return (false, "Соревнование не найдено или уже завершено.");

            var horse = await _context.Horses.FindAsync(dto.HorseId);
            if (horse?.StateOfHealth != "Здорова")
                return (false, "Данная лошадь не может участвовать в стартах по состоянию здоровья.");

            var participation = new Participation
            {
                IdCompetition = dto.CompetitionId,
                IdClient = dto.ClientId,
                IdHorse = dto.HorseId,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now), 
                ResultPlace = null, 
                Score = 0,
                StartNumber = null 
            };

            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return (true, "Вы успешно зарегистрированы на соревнования!");
        }
    }
}