using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Domain.Interfaces;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITicketPurchaseRepository _purchaseRepository;
    private readonly IEventRepository _eventRepository;

    public AdminController(IUserRepository userRepository, ITicketPurchaseRepository purchaseRepository, IEventRepository eventRepository)
    {
        _userRepository = userRepository;
        _purchaseRepository = purchaseRepository;
        _eventRepository = eventRepository;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();
        var result = users.Select(u => new { u.Id, u.Name, u.Email, Role = u.Role.ToString(), u.CreatedAt });
        return Ok(result);
    }

    [HttpGet("purchases")]
    public async Task<IActionResult> GetAllPurchases()
    {
        var purchases = await _purchaseRepository.GetAllAsync();
        var result = purchases.Select(p => new
        {
            p.Id,
            p.EventId,
            EventName = p.Event?.Name,
            p.TicketZoneId,
            ZoneName = p.TicketZone?.Name,
            p.UserId,
            UserName = p.User?.Name,
            p.Quantity,
            p.UnitPrice,
            p.TotalPrice,
            p.PurchaseDate
        });
        return Ok(result);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var events = await _eventRepository.GetAllAsync();
            var purchases = await _purchaseRepository.GetAllAsync();

            var totalRevenue = purchases.Sum(p => p.TotalPrice);
            var totalPurchases = purchases.Count();
            var totalEvents = events.Count();
            var totalUsers = users.Count();
            var activeEvents = events.Count(e => e.Status.ToString() == "Active");

            // Calcular ingresos por zona
            var revenueByZone = purchases
                .GroupBy(p => p.TicketZone?.Name ?? "Unknown")
                .Select(g => new
                {
                    zone = g.Key,
                    amount = Math.Round(g.Sum(p => p.TotalPrice), 2)
                })
                .OrderByDescending(x => x.amount)
                .ToList();

            // Top eventos por ingreso
            var topEvents = events
                .Select(e => new
                {
                    id = e.Id,
                    name = e.Name,
                    revenue = Math.Round(e.TicketPurchases.Sum(p => p.TotalPrice), 2),
                    ticketsSold = e.TicketPurchases.Sum(p => p.Quantity)
                })
                .OrderByDescending(e => e.revenue)
                .Take(10)
                .ToList();

            return Ok(new
            {
                totalRevenue = Math.Round(totalRevenue, 2),
                totalPurchases,
                totalEvents,
                activeEvents,
                totalUsers,
                revenueByZone,
                topEvents,
                recentPurchases = purchases
                    .OrderByDescending(p => p.PurchaseDate)
                    .Take(5)
                    .Select(p => new
                    {
                        p.Id,
                        eventName = p.Event?.Name,
                        zoneName = p.TicketZone?.Name,
                        p.Quantity,
                        p.TotalPrice,
                        purchaseDate = p.PurchaseDate
                    }),
                eventStats = events.Select(e => new
                {
                    e.Id,
                    e.Name,
                    status = e.Status.ToString(),
                    totalTicketsSold = e.TicketPurchases.Sum(p => p.Quantity),
                    revenue = Math.Round(e.TicketPurchases.Sum(p => p.TotalPrice), 2)
                })
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
