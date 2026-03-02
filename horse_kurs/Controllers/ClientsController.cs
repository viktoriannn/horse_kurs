using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using horse_kurs.Models;

namespace horse_kurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly EquestrianClubContext _context;

        public ClientsController(EquestrianClubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            client.DateOfRegistration = DateOnly.FromDateTime(DateTime.Now);

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return client;
        }
    }
}