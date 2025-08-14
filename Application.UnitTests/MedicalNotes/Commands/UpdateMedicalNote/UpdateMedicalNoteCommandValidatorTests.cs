using Application.MedicalNotes.Commands;
using Application.MedicalNotes.Commands.UpdateMedicalNote;
using FluentValidation.TestHelper;

namespace Application.UnitTests.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 2,
            Title = "Valid Title",
            Description = "Valid Description"
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
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 0,
            MedicalNoteId = 2,
            Title = "Valid Title",
            Description = "Valid Description"
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
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = -1,
            MedicalNoteId = 2,
            Title = "Valid Title",
            Description = "Valid Description"
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
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 0,
            Title = "Valid Title",
            Description = "Valid Description"
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
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = -1,
            Title = "Valid Title",
            Description = "Valid Description"
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.MedicalNoteId)
            .WithErrorMessage("Medical Note Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenTitleIsEmpty()
    {
        // GIVEN
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 2,
            Title = string.Empty,
            Description = "Valid Description"
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenTitleExceedsMaxLength()
    {
        // GIVEN
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 2,
            Title = new string('A', MedicalNoteValidatorsConstants.MaxTitleLength + 1),
            Description = "Valid Description"
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage($"Title must not exceed {MedicalNoteValidatorsConstants.MaxTitleLength} characters.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenDescriptionIsEmpty()
    {
        // GIVEN
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 2,
            Title = "Valid Title",
            Description = string.Empty
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenDescriptionExceedsMaxLength()
    {
        // GIVEN
        var validator = new UpdateMedicalNoteCommandValidator();
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 2,
            Title = "Valid Title",
            Description = new string('A', MedicalNoteValidatorsConstants.MaxDescriptionLength + 1)
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage($"Description must not exceed {MedicalNoteValidatorsConstants.MaxDescriptionLength} characters.");
    }
}
