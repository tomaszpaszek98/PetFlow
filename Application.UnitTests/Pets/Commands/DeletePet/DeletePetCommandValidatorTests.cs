using Application.Pets.Commands.DeletePet;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Pets.Commands.DeletePet;

public class DeletePetCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenPetIdIsGreaterThanZero()
    {
        // GIVEN
        var validator = new DeletePetCommandValidator();
        var command = new DeletePetCommand { PetId = 1 };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveValidationErrorFor(x => x.PetId);
    }

    [Test]
    public void ShouldHaveValidationErrorWhenPetIdIsZero()
    {
        // GIVEN
        var validator = new DeletePetCommandValidator();
        var command = new DeletePetCommand { PetId = 0 };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("PetId must be greater than zero.");
    }
    
    [Test]
    public void ShouldHaveValidationErrorWhenPetIdIsNegative()
    {
        // GIVEN
        var validator = new DeletePetCommandValidator();
        var command = new DeletePetCommand { PetId = -1 };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("PetId must be greater than zero.");
    }
}
