using MediatR;
using MiApp.Application.Features.TicketZones.Commands.AddTicketZone;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.TicketZones.Queries.GetZonesByEvent;

public class GetZonesByEventQuery : IRequest<IEnumerable<TicketZoneDto>>
{
    public int EventId { get; set; }
}

public class GetZonesByEventQueryHandler : IRequestHandler<GetZonesByEventQuery, IEnumerable<TicketZoneDto>>
{
    private readonly ITicketZoneRepository _zoneRepository;

    public GetZonesByEventQueryHandler(ITicketZoneRepository zoneRepository)
    {
        _zoneRepository = zoneRepository;
    }

    public async Task<IEnumerable<TicketZoneDto>> Handle(GetZonesByEventQuery request, CancellationToken cancellationToken)
    {
        var zones = await _zoneRepository.GetZonesByEventAsync(request.EventId);

        return zones.Select(z => new TicketZoneDto
        {
            Id = z.Id,
            EventId = z.EventId,
            Name = z.Name,
            Price = z.Price,
            TotalCapacity = z.TotalCapacity,
            AvailableTickets = z.AvailableTickets
        });
    }
}
