using Application.MedicalNotes.Queries.GetMedicalNoteDetails;
using FluentValidation.TestHelper;

namespace Application.UnitTests.MedicalNotes.Queries.GetMedicalNoteDetails;

public class GetMedicalNoteDetailsQueryValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new GetMedicalNoteDetailsQueryValidator();
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = 1,
            MedicalNoteId = 2
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
        var validator = new GetMedicalNoteDetailsQueryValidator();
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = 0,
            MedicalNoteId = 2
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
        var validator = new GetMedicalNoteDetailsQueryValidator();
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = -1,
            MedicalNoteId = 2
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenMedicalNoteIdIsZero()
    {
        // GIVEN
        var validator = new GetMedicalNoteDetailsQueryValidator();
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = 1,
            MedicalNoteId = 0
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.MedicalNoteId)
            .WithErrorMessage("Medical Note Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenMedicalNoteIdIsNegative()
    {
        // GIVEN
        var validator = new GetMedicalNoteDetailsQueryValidator();
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = 1,
            MedicalNoteId = -1
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.MedicalNoteId)
            .WithErrorMessage("Medical Note Id must be greater than zero.");
    }
}
