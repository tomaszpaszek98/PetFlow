using Application.Events.Commands.DeletePetFromEvent;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Events.Commands.DeletePetFromEvent;

public class DeletePetFromEventCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new DeletePetFromEventCommandValidator();
        var command = new DeletePetFromEventCommand
        {
            EventId = 1,
            PetId = 2
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveValidationErrorWhenEventIdIsZero()
    {
        // GIVEN
        var validator = new DeletePetFromEventCommandValidator();
        var command = new DeletePetFromEventCommand
        {
            EventId = 0,
            PetId = 2
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.EventId)
            .WithErrorMessage("Event Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenEventIdIsNegative()
    {
        // GIVEN
        var validator = new DeletePetFromEventCommandValidator();
        var command = new DeletePetFromEventCommand
        {
            EventId = -1,
            PetId = 2
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.EventId)
            .WithErrorMessage("Event Id must be greater than zero.");
    }
    
    [Test]
    public void ShouldHaveValidationErrorWhenPetIdIsZero()
    {
        // GIVEN
        var validator = new DeletePetFromEventCommandValidator();
        var command = new DeletePetFromEventCommand
        {
            EventId = 1,
            PetId = 0
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenPetIdIsNegative()
    {
        // GIVEN
        var validator = new DeletePetFromEventCommandValidator();
        var command = new DeletePetFromEventCommand
        {
            EventId = 1,
            PetId = -1
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }
}
