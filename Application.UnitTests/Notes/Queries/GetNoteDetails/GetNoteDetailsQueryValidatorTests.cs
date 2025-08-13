using Application.Notes.Queries.GetNoteDetails;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new GetNoteDetailsQueryValidator();
        var query = new GetNoteDetailsQuery
        {
            PetId = 1,
            NoteId = 2
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
        var validator = new GetNoteDetailsQueryValidator();
        var query = new GetNoteDetailsQuery
        {
            PetId = 0,
            NoteId = 2
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
        var validator = new GetNoteDetailsQueryValidator();
        var query = new GetNoteDetailsQuery
        {
            PetId = -1,
            NoteId = 2
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenNoteIdIsZero()
    {
        // GIVEN
        var validator = new GetNoteDetailsQueryValidator();
        var query = new GetNoteDetailsQuery
        {
            PetId = 1,
            NoteId = 0
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.NoteId)
            .WithErrorMessage("Note Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenNoteIdIsNegative()
    {
        // GIVEN
        var validator = new GetNoteDetailsQueryValidator();
        var query = new GetNoteDetailsQuery
        {
            PetId = 1,
            NoteId = -1
        };

        // WHEN
        var result = validator.TestValidate(query);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.NoteId)
            .WithErrorMessage("Note Id must be greater than zero.");
    }
}
