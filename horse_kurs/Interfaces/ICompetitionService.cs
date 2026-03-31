using horse_kurs.DTOs;

namespace horse_kurs.Interfaces
{
    public interface ICompetitionService
    {
        Task<(bool Success, string Message)> RegisterRiderAsync(RegisterParticipationDto dto);
    }
}