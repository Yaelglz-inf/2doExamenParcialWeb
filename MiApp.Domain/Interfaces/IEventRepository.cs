using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(int id);
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetActiveEventsAsync();
    Task AddAsync(Event @event);
    Task UpdateAsync(Event @event);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
