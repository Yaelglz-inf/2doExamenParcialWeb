using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

public interface ITicketZoneRepository
{
    Task<TicketZone?> GetByIdAsync(int id);
    Task<IEnumerable<TicketZone>> GetZonesByEventAsync(int eventId);
    Task AddAsync(TicketZone ticketZone);
    Task UpdateAsync(TicketZone ticketZone);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
