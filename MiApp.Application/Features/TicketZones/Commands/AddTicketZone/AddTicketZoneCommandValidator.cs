using FluentValidation;
using MiApp.Application.Features.TicketZones.Commands.AddTicketZone;

namespace MiApp.Application.Features.TicketZones.Commands.AddTicketZone;

public class AddTicketZoneCommandValidator : AbstractValidator<AddTicketZoneCommand>
{
    public AddTicketZoneCommandValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("El ID del evento es requerido");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la zona es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
            .LessThanOrEqualTo(99999.99m).WithMessage("El precio no puede exceder 99999.99");

        RuleFor(x => x.TotalCapacity)
            .GreaterThan(0).WithMessage("La capacidad debe ser mayor a 0")
            .LessThanOrEqualTo(10000).WithMessage("La capacidad no puede exceder 10000");
    }
}
