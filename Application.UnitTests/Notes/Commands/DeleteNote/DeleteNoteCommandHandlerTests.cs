using Application.Common.Interfaces.Repositories;
using Application.Notes.Commands.DeleteNote;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.UnitTests.Notes.Commands.DeleteNote;

public class DeleteNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldDeleteNoteSuccessfullyWhenNoteExistsAndBelongsToSpecificPet()
    {
        // GIVEN
        var command = new DeleteNoteCommand
        {
            PetId = 1,
            NoteId = 1
        };
        var note = new Note
        {
            Id = command.NoteId,
            PetId = command.PetId,
            Content = "Test Note Content",
            Type = NoteType.General
        };
        var repository = Substitute.For<INoteRepository>();
        var handler = new DeleteNoteCommandHandler(repository);
        
        repository.GetByIdAsync(command.NoteId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(note));
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);

        // THEN
        await repository.Received(1).DeleteByIdAsync(command.NoteId, Arg.Any<CancellationToken>());
    }
    
    [Test]
    public Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var command = new DeleteNoteCommand
        {
            PetId = 1,
            NoteId = 999
        };
        var repository = Substitute.For<INoteRepository>();
        var handler = new DeleteNoteCommandHandler(repository);
        
        repository.GetByIdAsync(command.NoteId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Note>(null));
        
        // WHEN/THEN
        var act = async () => await handler.Handle(command, CancellationToken.None);
        
        return FluentActions
            .Invoking(act)
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Note with ID {command.NoteId} does not exist.");
    }
    
    [Test]
    public Task ShouldThrowNotFoundExceptionWhenNoteDoesNotBelongToPet()
    {
        // GIVEN
        var petId = 1;
        var differentPetId = 2;
        var command = new DeleteNoteCommand
        {
            PetId = petId,
            NoteId = 1
        };
        var note = new Note
        {
            Id = command.NoteId,
            PetId = differentPetId,
            Content = "Test Note Content",
            Type = Domain.Enums.NoteType.General
        };
        var repository = Substitute.For<INoteRepository>();
        var handler = new DeleteNoteCommandHandler(repository);
        
        repository.GetByIdAsync(command.NoteId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(note));
        
        // WHEN/THEN
        var act = async () => await handler.Handle(command, CancellationToken.None);
        
        return FluentActions
            .Invoking(act)
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Note with ID {command.NoteId} does not belong to pet with ID {command.PetId}");
    }
}
