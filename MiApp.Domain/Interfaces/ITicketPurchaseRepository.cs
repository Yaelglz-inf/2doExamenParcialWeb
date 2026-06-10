using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

public interface ITicketPurchaseRepository
{
    Task<TicketPurchase?> GetByIdAsync(int id);
    Task<IEnumerable<TicketPurchase>> GetAllAsync();
    Task<IEnumerable<TicketPurchase>> GetPurchasesByEventAsync(int eventId);
    Task<IEnumerable<TicketPurchase>> GetPurchasesByUserAsync(int userId);
    Task AddAsync(TicketPurchase ticketPurchase);
    Task SaveChangesAsync();
}
