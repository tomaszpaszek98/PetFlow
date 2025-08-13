using Application.Common.Interfaces.Repositories;
using Application.Notes.Commands.UpdateNote;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnUpdatedNoteResponseWhenNoteIsUpdatedSuccessfully()
    {
        // GIVEN
        var request = new UpdateNoteCommand
        {
            PetId = 1,
            NoteId = 1,
            Content = "Updated Note Content",
            Type = NoteType.General
        };
        var pet = new Pet
        {
            Id = request.PetId,
            Name = "Test Pet"
        };
        var existingNote = new Note
        {
            Id = request.NoteId,
            PetId = request.PetId,
            Content = "Original Content",
            Type = NoteType.Behaviour,
            Created = DateTime.UtcNow.AddSeconds(-10)
        };
        var updatedNote = new Note
        {
            Id = request.NoteId,
            PetId = request.PetId,
            Content = request.Content,
            Type = request.Type,
            Created = existingNote.Created,
            Modified = DateTime.UtcNow
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository);
        
        petRepository.GetByIdAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pet));
        noteRepository.GetByIdAsync(request.NoteId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingNote));
        noteRepository.UpdateAsync(
                Arg.Is<Note>(n => n.Id == request.NoteId &&
                                n.PetId == request.PetId &&
                                n.Content == request.Content &&
                                n.Type == request.Type),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(updatedNote));
        
        // WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(updatedNote.Id);
        result.Content.Should().Be(request.Content);
        result.Type.Should().Be(request.Type);
        result.CreatedAt.Should().Be(existingNote.Created);
        
        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(request.PetId, Arg.Any<CancellationToken>());
            noteRepository.Received(1).GetByIdAsync(request.NoteId, Arg.Any<CancellationToken>());
            noteRepository.Received(1).UpdateAsync(
            Arg.Is<Note>(n => n.Id == request.NoteId &&
                              n.PetId == request.PetId &&
                              n.Content == request.Content &&
                              n.Type == request.Type),
            Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var request = new UpdateNoteCommand
        {
            PetId = 99,
            NoteId = 1,
            Content = "Updated Note Content",
            Type = NoteType.General
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository);
        
        petRepository.GetByIdAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(request, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(request.PetId.ToString()));
        await petRepository.Received(1).GetByIdAsync(request.PetId, Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().UpdateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var request = new UpdateNoteCommand
        {
            PetId = 1,
            NoteId = 99,
            Content = "Updated Note Content",
            Type = NoteType.General
        };
        var pet = new Pet
        {
            Id = request.PetId,
            Name = "Test Pet"
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository);
        
        petRepository.GetByIdAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pet));
        noteRepository.GetByIdAsync(request.NoteId, Arg.Any<CancellationToken>())
            .Returns((Note)null);
        
        // WHEN
        var act = () => handler.Handle(request, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Note)) && e.Message.Contains(request.NoteId.ToString()));
        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(request.PetId, Arg.Any<CancellationToken>());
            noteRepository.Received(1).GetByIdAsync(request.NoteId, Arg.Any<CancellationToken>());
        }); 
        await noteRepository.DidNotReceive().UpdateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotBelongToPet()
    {
        // GIVEN
        var petId = 1;
        var differentPetId = 2;
        var request = new UpdateNoteCommand
        {
            PetId = petId,
            NoteId = 1,
            Content = "Updated Note Content",
            Type = Domain.Enums.NoteType.General
        };
        var pet = new Pet
        {
            Id = request.PetId,
            Name = "Test Pet"
        };
        var existingNote = new Note
        {
            Id = request.NoteId,
            PetId = differentPetId,
            Content = "Original Content",
            Type = NoteType.Behaviour,
            Created = DateTime.UtcNow.AddDays(-1)
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository);
        
        petRepository.GetByIdAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pet));
        noteRepository.GetByIdAsync(request.NoteId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingNote));
        
        // WHEN
        var act = () => handler.Handle(request, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains("Note with ID") && 
                       e.Message.Contains(request.NoteId.ToString()) &&
                       e.Message.Contains("does not belong to pet with ID") &&
                       e.Message.Contains(request.PetId.ToString()));
        
        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(request.PetId, Arg.Any<CancellationToken>());
            noteRepository.Received(1).GetByIdAsync(request.NoteId, Arg.Any<CancellationToken>());
        });
        await noteRepository.DidNotReceive().UpdateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>());
    }
}
