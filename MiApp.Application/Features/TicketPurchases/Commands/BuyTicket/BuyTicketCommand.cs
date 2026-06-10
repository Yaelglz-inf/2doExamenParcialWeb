using MediatR;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.TicketPurchases.Commands.BuyTicket;

public class BuyTicketCommand : IRequest<TicketPurchaseDto>
{
    public int EventId { get; set; }
    public int TicketZoneId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
}

public class TicketPurchaseDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int TicketZoneId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class BuyTicketCommandHandler : IRequestHandler<BuyTicketCommand, TicketPurchaseDto>
{
    private readonly ITicketPurchaseRepository _purchaseRepository;
    private readonly ITicketZoneRepository _zoneRepository;

    public BuyTicketCommandHandler(
        ITicketPurchaseRepository purchaseRepository,
        ITicketZoneRepository zoneRepository)
    {
        _purchaseRepository = purchaseRepository;
        _zoneRepository = zoneRepository;
    }

    public async Task<TicketPurchaseDto> Handle(BuyTicketCommand request, CancellationToken cancellationToken)
    {
        // Obtener la zona de tickets
        var zone = await _zoneRepository.GetByIdAsync(request.TicketZoneId);
        if (zone == null)
            throw new Exception("Zona de tickets no encontrada");

        // Validar disponibilidad
        if (zone.AvailableTickets < request.Quantity)
            throw new Exception("No hay suficientes tickets disponibles");

        // Crear la compra
        var purchase = new TicketPurchase
        {
            EventId = request.EventId,
            TicketZoneId = request.TicketZoneId,
            UserId = request.UserId,
            Quantity = request.Quantity,
            UnitPrice = zone.Price,
            TotalPrice = zone.Price * request.Quantity
        };

        await _purchaseRepository.AddAsync(purchase);

        // Actualizar disponibilidad de tickets
        zone.AvailableTickets -= request.Quantity;
        await _zoneRepository.UpdateAsync(zone);

        return new TicketPurchaseDto
        {
            Id = purchase.Id,
            EventId = purchase.EventId,
            TicketZoneId = purchase.TicketZoneId,
            Quantity = purchase.Quantity,
            UnitPrice = purchase.UnitPrice,
            TotalPrice = purchase.TotalPrice,
            PurchaseDate = purchase.PurchaseDate
        };
    }
}
