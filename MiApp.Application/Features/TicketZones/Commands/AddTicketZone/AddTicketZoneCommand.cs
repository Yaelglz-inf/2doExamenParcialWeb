using MediatR;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.TicketZones.Commands.AddTicketZone;

public class AddTicketZoneCommand : IRequest<TicketZoneDto>
{
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalCapacity { get; set; }
}

public class TicketZoneDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableTickets { get; set; }
}

public class AddTicketZoneCommandHandler : IRequestHandler<AddTicketZoneCommand, TicketZoneDto>
{
    private readonly ITicketZoneRepository _zoneRepository;
    private readonly IEventRepository _eventRepository;

    public AddTicketZoneCommandHandler(
        ITicketZoneRepository zoneRepository,
        IEventRepository eventRepository)
    {
        _zoneRepository = zoneRepository;
        _eventRepository = eventRepository;
    }

    public async Task<TicketZoneDto> Handle(AddTicketZoneCommand request, CancellationToken cancellationToken)
    {
        // Validar que el evento existe
        var @event = await _eventRepository.GetByIdAsync(request.EventId);
        if (@event == null)
            throw new Exception("Evento no encontrado");

        var zone = new TicketZone
        {
            EventId = request.EventId,
            Name = request.Name,
            Price = request.Price,
            TotalCapacity = request.TotalCapacity,
            AvailableTickets = request.TotalCapacity
        };

        await _zoneRepository.AddAsync(zone);

        return new TicketZoneDto
        {
            Id = zone.Id,
            EventId = zone.EventId,
            Name = zone.Name,
            Price = zone.Price,
            TotalCapacity = zone.TotalCapacity,
            AvailableTickets = zone.AvailableTickets
        };
    }
}
