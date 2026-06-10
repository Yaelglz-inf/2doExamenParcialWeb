using MediatR;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands.CancelEvent;

public class CancelEventCommand : IRequest<bool>
{
    public int EventId { get; set; }
}

public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;

    public CancelEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<bool> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId);
        if (@event == null)
            return false;

        @event.Status = EventStatus.Cancelled;
        @event.UpdatedAt = DateTime.UtcNow;

        await _eventRepository.UpdateAsync(@event);
        return true;
    }
}
