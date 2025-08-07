using Application.Events.Queries.GetEventDetails;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenEventIdIsValid()
    {
        // GIVEN
        var validator = new GetEventDetailsQueryValidator();
        var query = new GetEventDetailsQuery
        {
            EventId = 1
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveValidationErrorWhenEventIdIsZero()
    {
        // GIVEN
        var validator = new GetEventDetailsQueryValidator();
        var query = new GetEventDetailsQuery
        {
            EventId = 0
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.EventId)
            .WithErrorMessage("Event Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenEventIdIsNegative()
    {
        // GIVEN
        var validator = new GetEventDetailsQueryValidator();
        var query = new GetEventDetailsQuery
        {
            EventId = -1
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.EventId)
            .WithErrorMessage("Event Id must be greater than zero.");
    }
}
