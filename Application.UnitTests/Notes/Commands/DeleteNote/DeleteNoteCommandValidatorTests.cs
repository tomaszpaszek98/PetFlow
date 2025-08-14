using Application.Notes.Commands.DeleteNote;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Notes.Commands.DeleteNote;

public class DeleteNoteCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new DeleteNoteCommandValidator();
        var command = new DeleteNoteCommand
        {
            PetId = 1,
            NoteId = 1
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveValidationErrorWhenPetIdIsZero()
    {
        // GIVEN
        var validator = new DeleteNoteCommandValidator();
        var command = new DeleteNoteCommand
        {
            PetId = 0,
            NoteId = 1
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
        var validator = new DeleteNoteCommandValidator();
        var command = new DeleteNoteCommand
        {
            PetId = -1,
            NoteId = 1
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenNoteIdIsZero()
    {
        // GIVEN
        var validator = new DeleteNoteCommandValidator();
        var command = new DeleteNoteCommand
        {
            PetId = 1,
            NoteId = 0
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.NoteId)
            .WithErrorMessage("Note Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenNoteIdIsNegative()
    {
        // GIVEN
        var validator = new DeleteNoteCommandValidator();
        var command = new DeleteNoteCommand
        {
            PetId = 1,
            NoteId = -1
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.NoteId)
            .WithErrorMessage("Note Id must be greater than zero.");
    }
}
