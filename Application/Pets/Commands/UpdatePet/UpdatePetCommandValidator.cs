using Domain.Constants;
using FluentValidation;

namespace Application.Pets.Commands.UpdatePet;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Pet name is required.")
            .MaximumLength(EntityConstants.Pet.MaxNameLength)
            .WithMessage($"Pet name must not exceed {EntityConstants.Pet.MaxNameLength} characters.");;
        RuleFor(x => x.Species)
            .NotEmpty()
            .WithMessage("Pet species is required.")
            .MaximumLength(EntityConstants.Pet.MaxSpeciesLength)
            .WithMessage($"Pet species must not exceed {EntityConstants.Pet.MaxSpeciesLength} characters.");;
        RuleFor(x => x.Breed)
            .NotEmpty()
            .WithMessage("Pet breed is required.")
            .MaximumLength(EntityConstants.Pet.MaxBreedLength)
            .WithMessage($"Pet breed must not exceed {EntityConstants.Pet.MaxBreedLength} characters.");
        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Date of birth cannot be in the future.");
    }
}