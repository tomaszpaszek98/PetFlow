using FluentValidation;

namespace Application.Pets.Commands.CreatePet;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Pet name is required.")
            .MaximumLength(50)
            .WithMessage("Pet name must not exceed 50 characters.");
        
        RuleFor(x => x.Species)
            .NotEmpty()
            .WithMessage("Pet species is required.")
            .MaximumLength(30)
            .WithMessage("Pet species must not exceed 30 characters.");
        
        RuleFor(x => x.Breed)
            .NotEmpty()
            .WithMessage("Pet breed is required.")
            .MaximumLength(30)
            .WithMessage("Pet breed must not exceed 30 characters.");
        
        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Date of birth cannot be in the future.");
    }
}