using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Events.Commands.CancelEvent;
using MiApp.Application.Features.Events.Commands.CreateEvent;
using MiApp.Application.Features.Events.Commands.EditEvent;
using MiApp.Application.Features.Events.Queries.GetAllEvents;
using MiApp.Application.Features.Events.Queries.GetEventById;
using MiApp.Application.Features.Events.Queries.SearchAndFilterEvents;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtener todos los eventos (activos por defecto)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllEvents([FromQuery] bool onlyActive = true)
    {
        try
        {
            var query = new GetAllEventsQuery { OnlyActive = onlyActive };
            var events = await _mediator.Send(query);
            return Ok(events);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Buscar y filtrar eventos
    /// </summary>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchAndFilterEvents(
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? location = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] bool? onlyActive = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new SearchAndFilterEventsQuery
            {
                SearchTerm = searchTerm,
                Location = location,
                StartDate = startDate,
                EndDate = endDate,
                OnlyActive = onlyActive,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var events = await _mediator.Send(query);
            return Ok(events);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener un evento por ID con sus zonas de tickets
    /// </summary>
    [HttpGet("{eventId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEventById(int eventId)
    {
        try
        {
            var query = new GetEventByIdQuery { EventId = eventId };
            var @event = await _mediator.Send(query);
            
            if (@event == null)
                return NotFound(new { error = "Evento no encontrado" });

            return Ok(@event);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Crear un nuevo evento (solo Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEventById), new { eventId = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Editar un evento existente (solo Admin)
    /// </summary>
    [HttpPut("{eventId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditEvent(int eventId, [FromBody] EditEventCommand command)
    {
        try
        {
            command.EventId = eventId;
            var result = await _mediator.Send(command);
            
            if (!result)
                return NotFound(new { error = "Evento no encontrado" });

            return Ok(new { message = "Evento actualizado correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Cancelar un evento (solo Admin)
    /// </summary>
    [HttpDelete("{eventId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CancelEvent(int eventId)
    {
        try
        {
            var command = new CancelEventCommand { EventId = eventId };
            var result = await _mediator.Send(command);
            
            if (!result)
                return NotFound(new { error = "Evento no encontrado" });

            return Ok(new { message = "Evento cancelado correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
