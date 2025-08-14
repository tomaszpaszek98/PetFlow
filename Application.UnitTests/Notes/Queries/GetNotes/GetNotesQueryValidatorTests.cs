using Application.Notes.Queries.GetNotes;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Notes.Queries.GetNotes;

public class GetNotesQueryValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenPetIdIsValid()
    {
        // GIVEN
        var validator = new GetNotesQueryValidator();
        var query = new GetNotesQuery
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
        var validator = new GetNotesQueryValidator();
        var query = new GetNotesQuery
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
        var validator = new GetNotesQueryValidator();
        var query = new GetNotesQuery
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
