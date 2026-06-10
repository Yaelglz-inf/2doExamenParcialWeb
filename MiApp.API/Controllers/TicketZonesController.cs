using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.TicketZones.Commands.AddTicketZone;
using MiApp.Application.Features.TicketZones.Commands.EditTicketZone;
using MiApp.Application.Features.TicketZones.Queries.GetZonesByEvent;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketZonesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketZonesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtener todas las zonas de un evento
    /// </summary>
    [HttpGet("event/{eventId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetZonesByEvent(int eventId)
    {
        try
        {
            var query = new GetZonesByEventQuery { EventId = eventId };
            var zones = await _mediator.Send(query);
            return Ok(zones);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Agregar una nueva zona de tickets (solo Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTicketZone([FromBody] AddTicketZoneCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetZonesByEvent), new { eventId = result.EventId }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Editar una zona de tickets (solo Admin)
    /// </summary>
    [HttpPut("{zoneId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditTicketZone(int zoneId, [FromBody] EditTicketZoneCommand command)
    {
        try
        {
            command.TicketZoneId = zoneId;
            var result = await _mediator.Send(command);
            
            if (!result)
                return NotFound(new { error = "Zona de tickets no encontrada" });

            return Ok(new { message = "Zona de tickets actualizada correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
