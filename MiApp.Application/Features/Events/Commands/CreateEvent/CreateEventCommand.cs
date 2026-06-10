using MediatR;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommand : IRequest<EventDto>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
}

public class TicketZoneDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableTickets { get; set; }
}

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<TicketZoneDto> TicketZones { get; set; } = new();
}

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = new Event
        {
            Name = request.Name,
            Description = request.Description,
            EventDate = request.EventDate,
            Location = request.Location,
            Status = EventStatus.Active,
            CreatedBy = request.CreatedBy
        };

        await _eventRepository.AddAsync(@event);

        return new EventDto
        {
            Id = @event.Id,
            Name = @event.Name,
            Description = @event.Description,
            EventDate = @event.EventDate,
            Location = @event.Location,
            Status = @event.Status.ToString(),
            TicketZones = new List<TicketZoneDto>()
        };
    }
}
