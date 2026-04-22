using horse_kurs.DTOs;

namespace horse_kurs.Interfaces
{
    public interface ICompetitionService
    {
        Task<List<CompetitionDto>> GetAllCompetitionsAsync();
        Task<CompetitionDto?> GetCompetitionByIdAsync(int id);
        Task<bool> RegisterParticipationAsync(RegisterParticipationDto dto);
        Task<List<ParticipationDto>> GetParticipationsByClientAsync(int clientId);
        Task<List<ParticipationDto>> GetParticipationsByCompetitionAsync(int competitionId);
    }
}