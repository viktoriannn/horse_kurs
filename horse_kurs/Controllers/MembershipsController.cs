using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipsController : ControllerBase
    {
        private readonly EquestrianClubContext _context;

        public MembershipsController(EquestrianClubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membership>>> GetMemberships()
        {
            return await _context.Memberships.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Membership>> GetMembership(int id)
        {
            var membership = await _context.Memberships.FindAsync(id);

            if (membership == null)
            {
                return NotFound();
            }

            return membership;
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<Membership>>> GetByClient(int clientId)
        {
            return await _context.Memberships
                .Where(m => m.IdClient == clientId) 
                .ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Membership>> PostMembership(Membership membership)
        {
            membership.PurchaseDate = DateOnly.FromDateTime(DateTime.Now);
            membership.ValidFrom = DateOnly.FromDateTime(DateTime.Now);


            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();

            return membership;
        }
    }
}