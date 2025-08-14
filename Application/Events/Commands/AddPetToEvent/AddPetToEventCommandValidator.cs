using FluentValidation;

namespace Application.Events.Commands.AddPetToEvent;

public class AddPetToEventCommandValidator : AbstractValidator<AddPetToEventCommand>
{
    public AddPetToEventCommandValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("Pet Id must be greater than zero.");
            
        RuleFor(x => x.EventId)
            .GreaterThan(0)
            .WithMessage("Event Id must be greater than zero.");
    }
}
