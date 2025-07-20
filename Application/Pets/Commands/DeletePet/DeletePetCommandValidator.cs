using FluentValidation;

namespace Application.Pets.Commands.DeletePet;

public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetCommandValidator()
    {
        RuleFor(x => x.PetId)
            .GreaterThan(0)
            .WithMessage("PetId must be greater than zero.");
    }
}

