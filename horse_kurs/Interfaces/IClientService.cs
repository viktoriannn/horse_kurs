using horse_kurs.DTOs;

namespace horse_kurs.Interfaces
{
    public interface IClientService
    {
        Task<ClientProfileDto?> GetProfileAsync(int clientId);
        Task<bool> TopUpBalanceAsync(int clientId, decimal amount);
        Task<bool> ChargeForLessonAsync(int clientId, int? membershipId = null);
    }
}