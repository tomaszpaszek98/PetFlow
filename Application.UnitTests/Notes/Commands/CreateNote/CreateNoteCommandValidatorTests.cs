using Application.Notes.Commands;
using Application.Notes.Commands.CreateNote;
using Domain.Entities;
using Domain.Enums;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Notes.Commands.CreateNote;

public class CreateNoteCommandValidatorTests
{
    [TestCase(NoteType.General)]
    [TestCase(NoteType.Behaviour)]
    [TestCase(NoteType.Mood)]
    [TestCase(NoteType.Symptom)]
    public void ShouldNotHaveValidationErrorWhenAllPropertiesAreValidForGivenType(NoteType type)
    {
        // GIVEN
        var validator = new CreateNoteCommandValidator();
        var command = new CreateNoteCommand
        {
            PetId = 1,
            Content = "Valid Content",
            Type = type
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
        var validator = new CreateNoteCommandValidator();
        var command = new CreateNoteCommand
        {
            PetId = 0,
            Content = "Valid Content",
            Type = NoteType.General
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
        var validator = new CreateNoteCommandValidator();
        var command = new CreateNoteCommand
        {
            PetId = -1,
            Content = "Valid Content",
            Type = NoteType.General
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.PetId)
            .WithErrorMessage("Pet Id must be greater than zero.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenContentIsEmpty()
    {
        // GIVEN
        var validator = new CreateNoteCommandValidator();
        var command = new CreateNoteCommand
        {
            PetId = 1,
            Content = "",
            Type = NoteType.General
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Content)
            .WithErrorMessage("Content is required.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenContentIsNull()
    {
        // GIVEN
        var validator = new CreateNoteCommandValidator();
        var command = new CreateNoteCommand
        {
            PetId = 1,
            Content = null,
            Type = NoteType.General
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Content)
            .WithErrorMessage("Content is required.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenContentExceedsMaxLength()
    {
        // GIVEN
        var validator = new CreateNoteCommandValidator();
        var command = new CreateNoteCommand
        {
            PetId = 1,
            Content = new string('x', NoteValidatorsConstants.MaxContentLength + 1),
            Type = NoteType.General
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        result.ShouldHaveValidationErrorFor(x => x.Content)
            .WithErrorMessage($"Content must not exceed {NoteValidatorsConstants.MaxContentLength} characters.");
    }

    [Test]
    public void ShouldHaveValidationErrorWhenTypeIsInvalid()
    {
        // GIVEN
        var invalidEnumValue = (NoteType)999;
        var validator = new CreateNoteCommandValidator();
        var command = new CreateNoteCommand
        {
            PetId = 1,
            Content = "Valid Content",
            Type = invalidEnumValue
        };

        // WHEN
        var result = validator.TestValidate(command);

        // THEN
        var validTypes = string.Join(", ", Enum.GetNames<NoteType>());
        result.ShouldHaveValidationErrorFor(x => x.Type)
            .WithErrorMessage($"Type must be a valid note type. Valid types: {validTypes}.");
    }
}
