using Application.MedicalNotes.Commands.DeleteMedicalNote;
using FluentValidation.TestHelper;

namespace Application.UnitTests.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new DeleteMedicalNoteCommandValidator();
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 1
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
        var validator = new DeleteMedicalNoteCommandValidator();
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 0,
            MedicalNoteId = 1
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
        var validator = new DeleteMedicalNoteCommandValidator();
        var command = new DeleteMedicalNoteCommand
        {
            PetId = -1,
            MedicalNoteId = 1
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenMedicalNoteIdIsZero()
    {
        // GIVEN
        var validator = new DeleteMedicalNoteCommandValidator();
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 0
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.MedicalNoteId)
            .WithErrorMessage("Medical Note Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenMedicalNoteIdIsNegative()
    {
        // GIVEN
        var validator = new DeleteMedicalNoteCommandValidator();
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = -1
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.MedicalNoteId)
            .WithErrorMessage("Medical Note Id must be greater than zero.");
    }
}
