using Application.MedicalNotes.Commands.CreateMedicalNote;
using FluentValidation.TestHelper;

namespace Application.UnitTests.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteCommandValidatorTests
{
    [Test]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValid()
    {
        // GIVEN
        var validator = new CreateMedicalNoteCommandValidator();
        var command = new CreateMedicalNoteCommand
        {
            PetId = 1,
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
        var validator = new CreateMedicalNoteCommandValidator();
        var command = new CreateMedicalNoteCommand
        {
            PetId = 0,
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
        var validator = new CreateMedicalNoteCommandValidator();
        var command = new CreateMedicalNoteCommand
        {
            PetId = -1,
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
    public void ShouldHaveValidationErrorWhenTitleIsEmpty()
    {
        // GIVEN
        var validator = new CreateMedicalNoteCommandValidator();
        var command = new CreateMedicalNoteCommand
        {
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
        var validator = new CreateMedicalNoteCommandValidator();
        var command = new CreateMedicalNoteCommand
        {
            Title = new string('A', 101), // 101 znaków przekracza limit 100
            Description = "Valid Description"
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must not exceed 100 characters.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenDescriptionIsEmpty()
    {
        // GIVEN
        var validator = new CreateMedicalNoteCommandValidator();
        var command = new CreateMedicalNoteCommand
        {
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
        var validator = new CreateMedicalNoteCommandValidator();
        var command = new CreateMedicalNoteCommand
        {
            Title = "Valid Title",
            Description = new string('A', 2001) // 2001 znaków przekracza limit 2000
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must not exceed 2000 characters.");
    }
}
