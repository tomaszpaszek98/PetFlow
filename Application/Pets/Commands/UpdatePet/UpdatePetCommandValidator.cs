using FluentValidation;

namespace Application.Pets.Commands.UpdatePet;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Species)
            .NotEmpty()
            .MaximumLength(30);
        RuleFor(x => x.Breed)
            .NotEmpty()
            .MaximumLength(30);
        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Date of birth cannot be in the future.");
    }
}

