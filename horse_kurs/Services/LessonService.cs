using horse_kurs.DTOs;
using horse_kurs.Interfaces;
using horse_kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Services
{
    public class LessonService : ILessonService
    {
        private readonly EquestrianClubContext _context;
        private readonly IClientService _clientService;

        public LessonService(EquestrianClubContext context, IClientService clientService)
        {
            _context = context;
            _clientService = clientService;
        }

        public async Task<IEnumerable<LessonViewDto>> GetScheduleAsync(DateTime date)
        {
            var targetDate = DateOnly.FromDateTime(date);

            return await _context.Lessons
                .Where(l => l.Date == targetDate) 
                .Select(l => new LessonViewDto
                {
                    Id = l.IdLesson,

                    Date = l.Date.ToDateTime(TimeOnly.MinValue),
                    Type = l.Type,
                    ClientName = l.IdClientNavigation.Surname,
                    CoachName = l.IdCoachNavigation.IdEmployeeNavigation.Surname,
                    HorseName = string.Join(", ", l.LessonHorses.Select(lh => lh.IdHorseNavigation.Name))
                }).ToListAsync();
        }

        public async Task<(bool Success, string Message)> BookLessonAsync(LessonCreateDto dto)
        {
            var bookingDate = DateOnly.FromDateTime(dto.Date);

            var coachBusy = await _context.Lessons
                .AnyAsync(l => l.IdCoach == dto.CoachId && l.Date == bookingDate);

            if (coachBusy) return (false, "Тренер уже занят в этот день.");

            var horse = await _context.Horses.FindAsync(dto.HorseId);
            if (horse == null || horse.StateOfHealth != "Здорова")
                return (false, "Лошадь недоступна по состоянию здоровья.");

            var charged = await _clientService.ChargeForLessonAsync(dto.ClientId);
            if (!charged) return (false, "Недостаточно средств или нет активного абонемента.");

            var lesson = new Lesson
            {
                Date = bookingDate, 
                Type = dto.Type,
                IdClient = dto.ClientId,
                IdCoach = dto.CoachId,
                IdArena = dto.ArenaId
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            var lessonHorse = new LessonHorse
            {
                IdLesson = lesson.IdLesson,
                IdHorse = dto.HorseId
            };
            _context.LessonHorses.Add(lessonHorse);

            await _context.SaveChangesAsync();
            return (true, "Занятие успешно забронировано.");
        }

        public async Task<bool> CancelLessonAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return false;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}