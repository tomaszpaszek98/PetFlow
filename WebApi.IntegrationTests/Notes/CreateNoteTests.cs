using Application.Notes.Commands.CreateNote;
using Domain.Enums;
using Domain.Exceptions;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Notes;

public class CreateNoteTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldCreateNoteWhenPetExists()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var noteContent = "New note content";
        var command = new CreateNoteCommand
        {
            PetId = petId,
            Content = noteContent,
            Type = NoteType.General
        };

        // WHEN
        var result = await Sender.Send(command);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Content.Should().Be(noteContent);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var command = new CreateNoteCommand
        {
            PetId = nonExistentPetId,
            Content = "Note content",
            Type = NoteType.Mood
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Pet*{nonExistentPetId}*");
    }

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var command = new CreateNoteCommand
        {
            PetId = petId,
            Content = string.Empty,
            Type = NoteType.Behaviour
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }
}

