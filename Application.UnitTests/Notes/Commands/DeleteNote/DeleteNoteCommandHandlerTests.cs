using Application.Common.Interfaces.Repositories;
using Application.Notes.Commands.DeleteNote;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldDeleteNoteWhenNoteExistsAndBelongsToPet()
    {
        // GIVEN
        var command = new DeleteNoteCommand
        {
            PetId = 1,
            NoteId = 1
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeleteNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<DeleteNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.DeleteAsync(command.NoteId, command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);

        // THEN
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            noteRepository.DeleteAsync(command.NoteId, command.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var command = new DeleteNoteCommand
        {
            PetId = 1,
            NoteId = 999
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeleteNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<DeleteNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.DeleteAsync(command.NoteId, command.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Note)) && e.Message.Contains(command.NoteId.ToString()));
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            noteRepository.DeleteAsync(command.NoteId, command.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new DeleteNoteCommand
        {
            PetId = 999,
            NoteId = 1
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeleteNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<DeleteNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().DeleteAsync(default, default);
    }
}
