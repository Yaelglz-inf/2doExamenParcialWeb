using MediatR;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.TicketZones.Commands.EditTicketZone;

public class EditTicketZoneCommand : IRequest<bool>
{
    public int TicketZoneId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalCapacity { get; set; }
}

public class EditTicketZoneCommandHandler : IRequestHandler<EditTicketZoneCommand, bool>
{
    private readonly ITicketZoneRepository _zoneRepository;

    public EditTicketZoneCommandHandler(ITicketZoneRepository zoneRepository)
    {
        _zoneRepository = zoneRepository;
    }

    public async Task<bool> Handle(EditTicketZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = await _zoneRepository.GetByIdAsync(request.TicketZoneId);
        if (zone == null)
            return false;

        zone.Name = request.Name;
        zone.Price = request.Price;
        
        // Si la capacidad total aumenta, aumentar tickets disponibles
        if (request.TotalCapacity > zone.TotalCapacity)
        {
            int difference = request.TotalCapacity - zone.TotalCapacity;
            zone.AvailableTickets += difference;
        }
        // Si disminuye, no se puede si hay tickets vendidos
        else if (request.TotalCapacity < zone.TotalCapacity)
        {
            int ticketsSold = zone.TotalCapacity - zone.AvailableTickets;
            if (request.TotalCapacity < ticketsSold)
                throw new Exception("No se puede reducir la capacidad por debajo de los tickets ya vendidos");
            zone.AvailableTickets = request.TotalCapacity - ticketsSold;
        }

        zone.TotalCapacity = request.TotalCapacity;
        zone.UpdatedAt = DateTime.UtcNow;

        await _zoneRepository.UpdateAsync(zone);
        return true;
    }
}
