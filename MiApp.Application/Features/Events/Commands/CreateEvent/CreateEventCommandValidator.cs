using FluentValidation;
using MiApp.Application.Features.Events.Commands.CreateEvent;

namespace MiApp.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del evento es requerido")
            .MaximumLength(256).WithMessage("El nombre no puede exceder 256 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es requerida")
            .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres");

        RuleFor(x => x.EventDate)
            .NotEmpty().WithMessage("La fecha del evento es requerida")
            .GreaterThan(DateTime.UtcNow).WithMessage("La fecha del evento debe ser futura");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("La ubicación es requerida")
            .MaximumLength(500).WithMessage("La ubicación no puede exceder 500 caracteres");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("El ID del creador es requerido");
    }
}
