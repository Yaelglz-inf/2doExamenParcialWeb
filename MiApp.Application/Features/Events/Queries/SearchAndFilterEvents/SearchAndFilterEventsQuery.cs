using MediatR;
using MiApp.Application.Features.Events.Commands.CreateEvent;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries.SearchAndFilterEvents;

public class SearchAndFilterEventsQuery : IRequest<IEnumerable<EventDto>>
{
    public string? SearchTerm { get; set; } // Buscar en nombre y descripción
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? OnlyActive { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SearchAndFilterEventsQueryHandler : IRequestHandler<SearchAndFilterEventsQuery, IEnumerable<EventDto>>
{
    private readonly IEventRepository _eventRepository;

    public SearchAndFilterEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventDto>> Handle(SearchAndFilterEventsQuery request, CancellationToken cancellationToken)
    {
        var events = request.OnlyActive.HasValue && request.OnlyActive.Value
            ? await _eventRepository.GetActiveEventsAsync()
            : await _eventRepository.GetAllAsync();

        // Filtrar por término de búsqueda
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            events = events.Where(e => 
                e.Name.ToLower().Contains(searchLower) ||
                e.Description.ToLower().Contains(searchLower)
            );
        }

        // Filtrar por ubicación
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            var locationLower = request.Location.ToLower();
            events = events.Where(e => e.Location.ToLower().Contains(locationLower));
        }

        // Filtrar por rango de fechas
        if (request.StartDate.HasValue)
        {
            events = events.Where(e => e.EventDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            events = events.Where(e => e.EventDate <= request.EndDate.Value);
        }

        // Paginación
        events = events
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            EventDate = e.EventDate,
            Location = e.Location,
            Status = e.Status.ToString()
        });
    }
}
