using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.TicketZones)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.TicketZones)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetActiveEventsAsync()
    {
        return await _context.Events
            .Where(e => e.Status == EventStatus.Active)
            .Include(e => e.TicketZones)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }

    public async Task AddAsync(Event @event)
    {
        await _context.Events.AddAsync(@event);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Event @event)
    {
        _context.Events.Update(@event);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var @event = await GetByIdAsync(id);
        if (@event != null)
        {
            _context.Events.Remove(@event);
            await SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
