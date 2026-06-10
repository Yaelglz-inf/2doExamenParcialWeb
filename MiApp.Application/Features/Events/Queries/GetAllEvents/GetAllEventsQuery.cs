using MediatR;
using MiApp.Application.Features.Events.Commands.CreateEvent;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries.GetAllEvents;

public class GetAllEventsQuery : IRequest<IEnumerable<EventDto>>
{
    public bool OnlyActive { get; set; } = true;
}

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, IEnumerable<EventDto>>
{
    private readonly IEventRepository _eventRepository;

    public GetAllEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var events = request.OnlyActive
            ? await _eventRepository.GetActiveEventsAsync()
            : await _eventRepository.GetAllAsync();

        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            EventDate = e.EventDate,
            Location = e.Location,
            Status = e.Status.ToString(),
            TicketZones = e.TicketZones.Select(z => new TicketZoneDto
            {
                Id = z.Id,
                Name = z.Name,
                Price = z.Price,
                TotalCapacity = z.TotalCapacity,
                AvailableTickets = z.AvailableTickets
            }).ToList()
        });
    }
}
