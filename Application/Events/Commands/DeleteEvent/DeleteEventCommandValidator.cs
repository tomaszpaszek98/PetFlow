using FluentValidation;

namespace Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0)
            .WithMessage("EventId must be greater than zero.");
    }
}
