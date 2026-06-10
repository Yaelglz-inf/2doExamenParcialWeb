using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

public class TicketZoneRepository : ITicketZoneRepository
{
    private readonly AppDbContext _context;

    public TicketZoneRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TicketZone?> GetByIdAsync(int id)
    {
        return await _context.TicketZones.FirstOrDefaultAsync(tz => tz.Id == id);
    }

    public async Task<IEnumerable<TicketZone>> GetZonesByEventAsync(int eventId)
    {
        return await _context.TicketZones
            .Where(tz => tz.EventId == eventId)
            .OrderBy(tz => tz.Name)
            .ToListAsync();
    }

    public async Task AddAsync(TicketZone ticketZone)
    {
        await _context.TicketZones.AddAsync(ticketZone);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(TicketZone ticketZone)
    {
        _context.TicketZones.Update(ticketZone);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var zone = await GetByIdAsync(id);
        if (zone != null)
        {
            _context.TicketZones.Remove(zone);
            await SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
