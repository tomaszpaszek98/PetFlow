using Application.Pets.Queries.GetPetDetails;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenPetIdIsValid()
    {
        // GIVEN
        var validator = new GetPetDetailsQueryValidator();
        var query = new GetPetDetailsQuery
        {
            PetId = 1
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveValidationErrorWhenPetIdIsZero()
    {
        // GIVEN
        var validator = new GetPetDetailsQueryValidator();
        var query = new GetPetDetailsQuery
        {
            PetId = 0
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenPetIdIsNegative()
    {
        // GIVEN
        var validator = new GetPetDetailsQueryValidator();
        var query = new GetPetDetailsQuery
        {
            PetId = -1
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }
}
