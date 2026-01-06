using Application.Notes.Commands.UpdateNote;
using Domain.Enums;
using Domain.Exceptions;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Notes;

public class UpdateNoteTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldUpdateNoteWhenNoteAndPetExist()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var noteId = 1; // Note from seed data for pet #1
        var updatedContent = "Updated note content";
        var updateCommand = new UpdateNoteCommand
        {
            PetId = petId,
            NoteId = noteId,
            Content = updatedContent,
            Type = NoteType.Symptom
        };

        // WHEN
        var result = await Sender.Send(updateCommand);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(noteId);
        result.Content.Should().Be(updatedContent);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var petId = 1;
        var nonExistentNoteId = 999999;
        var updateCommand = new UpdateNoteCommand
        {
            PetId = petId,
            NoteId = nonExistentNoteId,
            Content = "Content",
            Type = NoteType.General
        };

        // WHEN
        var act = async () => await Sender.Send(updateCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Note*{nonExistentNoteId}*");
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var noteId = 1;
        var updateCommand = new UpdateNoteCommand
        {
            PetId = nonExistentPetId,
            NoteId = noteId,
            Content = "Content",
            Type = NoteType.General
        };

        // WHEN
        var act = async () => await Sender.Send(updateCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Pet*{nonExistentPetId}*");
    }

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var petId = 2; // Pet from seed data
        var noteId = 2; // Note from seed data for pet #2
        var invalidUpdateCommand = new UpdateNoteCommand
        {
            PetId = petId,
            NoteId = noteId,
            Content = string.Empty,
            Type = NoteType.Behaviour
        };

        // WHEN
        var act = async () => await Sender.Send(invalidUpdateCommand);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }
}

