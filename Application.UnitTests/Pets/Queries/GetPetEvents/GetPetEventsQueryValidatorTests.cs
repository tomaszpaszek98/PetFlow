using Application.Pets.Queries.GetPetEvents;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Pets.Queries.GetPetEvents;

public class GetPetEventsQueryValidatorTests
{
    [Test]
    public void ShouldReturnValidResultWhenPetIdIsPositive()
    {
        // GIVEN
        var query = new GetPetEventsQuery { PetId = 1 };
        var validator = new GetPetEventsQueryValidator();

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldReturnValidationErrorResultWhenPetIdIsZero()
    {
        // GIVEN
        var query = new GetPetEventsQuery { PetId = 0 };
        var validator = new GetPetEventsQueryValidator();

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId);
    }

    [Test]
    public void ShouldReturnValidationErrorResultWhenPetIdIsNegative()
    {
        // GIVEN
        var query = new GetPetEventsQuery { PetId = -5 };
        var validator = new GetPetEventsQueryValidator();

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId);
    }
}