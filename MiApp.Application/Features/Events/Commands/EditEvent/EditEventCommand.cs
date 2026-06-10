using MediatR;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands.EditEvent;

public class EditEventCommand : IRequest<bool>
{
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class EditEventCommandHandler : IRequestHandler<EditEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;

    public EditEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<bool> Handle(EditEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId);
        if (@event == null)
            return false;

        @event.Name = request.Name;
        @event.Description = request.Description;
        @event.EventDate = request.EventDate;
        @event.Location = request.Location;
        @event.UpdatedAt = DateTime.UtcNow;

        await _eventRepository.UpdateAsync(@event);
        return true;
    }
}
