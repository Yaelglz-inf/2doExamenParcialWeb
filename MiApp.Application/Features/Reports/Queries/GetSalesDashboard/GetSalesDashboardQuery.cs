using MediatR;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Reports.Queries.GetSalesDashboard;

public class GetSalesDashboardQuery : IRequest<SalesDashboardDto>
{
    public int? EventId { get; set; } // Si es null, trae todas
}

public class SalesDashboardDto
{
    public int TotalEventos { get; set; }
    public int TotalTicketsVendidos { get; set; }
    public decimal VentasTotales { get; set; }
    public List<EventSalesDto> EventosPorVentas { get; set; } = new();
}

public class EventSalesDto
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int TicketsVendidos { get; set; }
    public decimal VentasTotales { get; set; }
    public List<ZoneSalesDto> ZonesSales { get; set; } = new();
}

public class ZoneSalesDto
{
    public int ZoneId { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableTickets { get; set; }
    public int TicketsVendidos { get; set; }
}

public class GetSalesDashboardQueryHandler : IRequestHandler<GetSalesDashboardQuery, SalesDashboardDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly ITicketPurchaseRepository _purchaseRepository;

    public GetSalesDashboardQueryHandler(
        IEventRepository eventRepository,
        ITicketPurchaseRepository purchaseRepository)
    {
        _eventRepository = eventRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<SalesDashboardDto> Handle(GetSalesDashboardQuery request, CancellationToken cancellationToken)
    {
        var events = request.EventId.HasValue
            ? new[] { await _eventRepository.GetByIdAsync(request.EventId.Value) }.Where(e => e != null).ToList()
            : (await _eventRepository.GetAllAsync()).ToList();

        var dashboard = new SalesDashboardDto
        {
            TotalEventos = events.Count
        };

        foreach (var @event in events)
        {
            var eventPurchases = await _purchaseRepository.GetPurchasesByEventAsync(@event.Id);
            var eventSales = new EventSalesDto
            {
                EventId = @event.Id,
                EventName = @event.Name,
                EventDate = @event.EventDate,
                TicketsVendidos = eventPurchases.Sum(p => p.Quantity),
                VentasTotales = eventPurchases.Sum(p => p.TotalPrice)
            };

            // Detalles por zona
            foreach (var zone in @event.TicketZones)
            {
                var zonePurchases = eventPurchases.Where(p => p.TicketZoneId == zone.Id).ToList();
                eventSales.ZonesSales.Add(new ZoneSalesDto
                {
                    ZoneId = zone.Id,
                    ZoneName = zone.Name,
                    Price = zone.Price,
                    TotalCapacity = zone.TotalCapacity,
                    AvailableTickets = zone.AvailableTickets,
                    TicketsVendidos = zonePurchases.Sum(p => p.Quantity)
                });
            }

            dashboard.EventosPorVentas.Add(eventSales);
            dashboard.TotalTicketsVendidos += eventSales.TicketsVendidos;
            dashboard.VentasTotales += eventSales.VentasTotales;
        }

        return dashboard;
    }
}
