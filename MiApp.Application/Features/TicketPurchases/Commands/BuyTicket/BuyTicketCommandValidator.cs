using FluentValidation;
using MiApp.Application.Features.TicketPurchases.Commands.BuyTicket;

namespace MiApp.Application.Features.TicketPurchases.Commands.BuyTicket;

public class BuyTicketCommandValidator : AbstractValidator<BuyTicketCommand>
{
    public BuyTicketCommandValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("El ID del evento es requerido");

        RuleFor(x => x.TicketZoneId)
            .GreaterThan(0).WithMessage("El ID de la zona es requerido");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("El ID del usuario es requerido");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0")
            .LessThanOrEqualTo(100).WithMessage("No puedes comprar más de 100 tickets a la vez");
    }
}
