using FluentValidation;

namespace Application.Pets.Commands.UpdatePet;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    
    public UpdatePetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Pet name is required.")
            .MaximumLength(PetValidatorsConstants.MaxNameLength)
            .WithMessage($"Pet name must not exceed {PetValidatorsConstants.MaxNameLength} characters.");;
        RuleFor(x => x.Species)
            .NotEmpty()
            .WithMessage("Pet species is required.")
            .MaximumLength(PetValidatorsConstants.MaxSpeciesLength)
            .WithMessage($"Pet species must not exceed {PetValidatorsConstants.MaxSpeciesLength} characters.");;
        RuleFor(x => x.Breed)
            .NotEmpty()
            .WithMessage("Pet breed is required.")
            .MaximumLength(PetValidatorsConstants.MaxBreedLength)
            .WithMessage($"Pet breed must not exceed {PetValidatorsConstants.MaxBreedLength} characters.");
        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Date of birth cannot be in the future.");
    }
}