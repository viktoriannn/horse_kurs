using horse_kurs.DTOs;

namespace horse_kurs.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonViewDto>> GetScheduleAsync(DateTime date);
        Task<(bool Success, string Message)> BookLessonAsync(LessonCreateDto dto);
        Task<bool> CancelLessonAsync(int id);
    }
}