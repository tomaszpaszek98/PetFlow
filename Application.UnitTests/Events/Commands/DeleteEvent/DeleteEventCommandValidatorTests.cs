using Application.Events.Commands.DeleteEvent;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Events.Commands.DeleteEvent;

public class DeleteEventCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenEventIdIsGreaterThanZero()
    {
        // GIVEN
        var validator = new DeleteEventCommandValidator();
        var command = new DeleteEventCommand { EventId = 1 };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveValidationErrorFor(x => x.EventId);
    }

    [Test]
    public void ShouldHaveValidationErrorWhenEventIdIsZero()
    {
        // GIVEN
        var validator = new DeleteEventCommandValidator();
        var command = new DeleteEventCommand { EventId = 0 };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.EventId)
            .WithErrorMessage("EventId must be greater than zero.");
    }
    
    [Test]
    public void ShouldHaveValidationErrorWhenEventIdIsNegative()
    {
        // GIVEN
        var validator = new DeleteEventCommandValidator();
        var command = new DeleteEventCommand { EventId = -1 };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.EventId)
            .WithErrorMessage("EventId must be greater than zero.");
    }
}
