using Application.MedicalNotes.Queries.GetMedicalNotes;
using FluentValidation.TestHelper;

namespace Application.UnitTests.MedicalNotes.Queries.GetMedicalNotes;

public class GetMedicalNotesQueryValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenPetIdIsValid()
    {
        // GIVEN
        var validator = new GetMedicalNotesQueryValidator();
        var query = new GetMedicalNotesQuery
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
        var validator = new GetMedicalNotesQueryValidator();
        var query = new GetMedicalNotesQuery
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
        var validator = new GetMedicalNotesQueryValidator();
        var query = new GetMedicalNotesQuery
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
