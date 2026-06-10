using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

public class TicketPurchaseRepository : ITicketPurchaseRepository
{
    private readonly AppDbContext _context;

    public TicketPurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TicketPurchase?> GetByIdAsync(int id)
    {
        return await _context.TicketPurchases
            .Include(tp => tp.Event)
            .Include(tp => tp.TicketZone)
            .Include(tp => tp.User)
            .FirstOrDefaultAsync(tp => tp.Id == id);
    }

    public async Task<IEnumerable<TicketPurchase>> GetAllAsync()
    {
        return await _context.TicketPurchases
            .Include(tp => tp.Event)
            .Include(tp => tp.TicketZone)
            .Include(tp => tp.User)
            .OrderByDescending(tp => tp.PurchaseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketPurchase>> GetPurchasesByEventAsync(int eventId)
    {
        return await _context.TicketPurchases
            .Where(tp => tp.EventId == eventId)
            .Include(tp => tp.User)
            .Include(tp => tp.TicketZone)
            .OrderByDescending(tp => tp.PurchaseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketPurchase>> GetPurchasesByUserAsync(int userId)
    {
        return await _context.TicketPurchases
            .Where(tp => tp.UserId == userId)
            .Include(tp => tp.Event)
            .Include(tp => tp.TicketZone)
            .OrderByDescending(tp => tp.PurchaseDate)
            .ToListAsync();
    }

    public async Task AddAsync(TicketPurchase ticketPurchase)
    {
        await _context.TicketPurchases.AddAsync(ticketPurchase);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
