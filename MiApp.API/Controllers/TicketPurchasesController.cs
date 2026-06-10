using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.TicketPurchases.Commands.BuyTicket;
using MiApp.Application.Features.TicketPurchases.Queries.GetUserTicketPurchases;

namespace MiApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketPurchasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketPurchasesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Comprar tickets de un evento
    /// </summary>
    [HttpPost("buy")]
    [AllowAnonymous]
    public async Task<IActionResult> BuyTickets([FromBody] BuyTicketCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new 
            { 
                message = "Tickets comprados exitosamente",
                purchase = result
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener mis compras de tickets (para el usuario actual)
    /// </summary>
    [HttpGet("my-purchases")]
    public async Task<IActionResult> GetMyPurchases()
    {
        try
        {
            // Obtener el UserId del JWT
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
