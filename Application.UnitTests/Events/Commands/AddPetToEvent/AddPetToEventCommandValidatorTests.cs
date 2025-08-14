using Application.Events.Commands.AddPetToEvent;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Events.Commands.AddPetToEvent;

public class AddPetToEventCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new AddPetToEventCommandValidator();
        var command = new AddPetToEventCommand
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
        var validator = new AddPetToEventCommandValidator();
        var command = new AddPetToEventCommand
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
        var validator = new AddPetToEventCommandValidator();
        var command = new AddPetToEventCommand
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
        var validator = new AddPetToEventCommandValidator();
        var command = new AddPetToEventCommand
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
        var validator = new AddPetToEventCommandValidator();
        var command = new AddPetToEventCommand
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

