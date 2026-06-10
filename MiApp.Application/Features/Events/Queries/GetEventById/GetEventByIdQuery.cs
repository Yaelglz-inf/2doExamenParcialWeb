using MediatR;
using MiApp.Application.Features.Events.Commands.CreateEvent;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries.GetEventById;

public class GetEventByIdQuery : IRequest<EventDto?>
{
    public int EventId { get; set; }
}

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto?>
{
    private readonly IEventRepository _eventRepository;

    public GetEventByIdQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId);
        if (@event == null)
            return null;

        return new EventDto
        {
            Id = @event.Id,
            Name = @event.Name,
            Description = @event.Description,
            EventDate = @event.EventDate,
            Location = @event.Location,
            Status = @event.Status.ToString()
        };
    }
}
