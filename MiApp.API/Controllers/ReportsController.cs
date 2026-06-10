using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Reports.Queries.GetSalesDashboard;
using MiApp.Application.Features.TicketPurchases.Queries.GetUserTicketPurchases;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtener dashboard de ventas (solo Admin)
    /// </summary>
    [HttpGet("sales-dashboard")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSalesDashboard([FromQuery] int? eventId = null)
    {
        try
        {
            var query = new GetSalesDashboardQuery { EventId = eventId };
            var dashboard = await _mediator.Send(query);
            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener mis compras de tickets (para el usuario actual)
    /// </summary>
    [HttpGet("my-tickets")]
    [Authorize]
    public async Task<IActionResult> GetMyTickets()
    {
        try
        {
            // Obtener el UserId del JWT (esto requiere configurar en AuthController)
            var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { error = "No se pudo obtener el ID del usuario" });

            var query = new GetUserTicketPurchasesQuery { UserId = userId };
            var purchases = await _mediator.Send(query);
            return Ok(purchases);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
