using Application.Notes.Commands.DeleteNote;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Notes;

public class DeleteNoteTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldDeleteNoteWhenNoteAndPetExist()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var noteId = 1; // Note from seed data for pet #1
        var deleteCommand = new DeleteNoteCommand
        {
            PetId = petId,
            NoteId = noteId
        };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var petId = 1;
        var nonExistentNoteId = 999999;
        var deleteCommand = new DeleteNoteCommand
        {
            PetId = petId,
            NoteId = nonExistentNoteId
        };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

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
        var deleteCommand = new DeleteNoteCommand
        {
            PetId = nonExistentPetId,
            NoteId = noteId
        };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Pet*{nonExistentPetId}*");
    }
}

