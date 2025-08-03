using Application.Pets.Queries.GetPetEvents;

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
        var result = validator.Validate(query);

        // THEN
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void ShouldReturnValidationErrorResultWhenPetIdIsZero()
    {
        // GIVEN
        var query = new GetPetEventsQuery { PetId = 0 };
        var validator = new GetPetEventsQueryValidator();

        // WHEN
        var result = validator.Validate(query);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "PetId");
    }

    [Test]
    public void ShouldReturnValidationErrorResultWhenPetIdIsNegative()
    {
        // GIVEN
        var query = new GetPetEventsQuery { PetId = -5 };
        var validator = new GetPetEventsQueryValidator();

        // WHEN
        var result = validator.Validate(query);

        // THEN
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "PetId");
    }
}