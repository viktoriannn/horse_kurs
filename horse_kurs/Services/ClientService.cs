using horse_kurs.DTOs;
using horse_kurs.Interfaces;
using horse_kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Services
{
    public class ClientService : IClientService
    {
        private readonly EquestrianClubContext _context;

        public ClientService(EquestrianClubContext context)
        {
            _context = context;
        }

        public async Task<ClientProfileDto?> GetProfileAsync(int clientId)
        {
            var client = await _context.Clients
                .Include(c => c.Memberships)
                .FirstOrDefaultAsync(c => c.IdClient == clientId);

            if (client == null) return null;

            return new ClientProfileDto
            {
                IdClient = client.IdClient,
                FullName = $"{client.Surname} {client.Name} {client.Patronymic}",
                Phone = client.Phone,
                Balance = client.Balance,
                ActiveMemberships = client.Memberships
                    .Where(m => m.Status == "Активен" && m.ValidUntil >= DateOnly.FromDateTime(DateTime.Now))
                    .Select(m => new MembershipDto
                    {
                        Id = m.IdMembership,
                        Type = m.Type,
                        RemainingLessons = m.LessonsTotal,
                        ValidUntil = m.ValidUntil,
                        Status = m.Status
                    }).ToList(),
                UpcomingLessons = new List<ClientLessonDto>() 
            };
        }

        public async Task<bool> TopUpBalanceAsync(int clientId, decimal amount)
        {
            var client = await _context.Clients
                .Include(c => c.Memberships)
                .FirstOrDefaultAsync(c => c.IdClient == clientId);

            if (client == null) return false;

            client.Balance += amount;

            var activeMembership = client.Memberships
                .OrderByDescending(m => m.IdMembership)
                .FirstOrDefault();

            if (activeMembership == null)
            {
                await _context.SaveChangesAsync();
                return true;
            }

            var payment = new Payment
            {
                Summa = amount,
                MethodPaid = "Карта",
                PaymentDate = DateTime.Now,
                Status = "Завершено",
                IdMembership = activeMembership.IdMembership
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChargeForLessonAsync(int clientId, int? membershipId = null)
        {
            var client = await _context.Clients
                .Include(c => c.Memberships)
                .FirstOrDefaultAsync(c => c.IdClient == clientId);

            if (client == null) return false;

            if (membershipId.HasValue)
            {
                var membership = client.Memberships.FirstOrDefault(m => m.IdMembership == membershipId);
                if (membership != null && membership.LessonsTotal > 0)
                {
                    membership.LessonsTotal--;
                    if (membership.LessonsTotal == 0) membership.Status = "Использован";
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            const decimal lessonPrice = 2000m;
            if (client.Balance >= lessonPrice)
            {
                client.Balance -= lessonPrice;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}