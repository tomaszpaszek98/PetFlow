using Application.Common.Interfaces.Repositories;
using Application.Notes.Commands.UpdateNote;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

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
            Created = DateTime.UtcNow.AddSeconds(-10),
            Pet = pet
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository);
        
        petRepository.ExistsAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingNote));
        noteRepository.UpdateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(existingNote.Id);
        result.Content.Should().Be(request.Content);
        result.Type.Should().Be(request.Type);
        result.CreatedAt.Should().Be(existingNote.Created);
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(request.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, Arg.Any<CancellationToken>());
            noteRepository.UpdateAsync(
                Arg.Is<Note>(n => n.Id == request.NoteId &&
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
        
        petRepository.ExistsAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(request, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(request.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(request.PetId, Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().GetByIdWithPetAsync(default, default);
        await noteRepository.DidNotReceive().UpdateAsync(default);
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
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository);
        
        petRepository.ExistsAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Note>(null));
        
        // WHEN
        var act = async () => await handler.Handle(request, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Note)) && e.Message.Contains(request.NoteId.ToString()));
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(request.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, Arg.Any<CancellationToken>());
        });
        await noteRepository.DidNotReceive().UpdateAsync(default);
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotBelongToPet()
    {
        // GIVEN
        var request = new UpdateNoteCommand
        {
            PetId = 1,
            NoteId = 1,
            Content = "Updated Note Content",
            Type = NoteType.General
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository);
        
        petRepository.ExistsAsync(request.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Note>(null));
        
        // WHEN
        var act = async () => await handler.Handle(request, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(request.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetByIdWithPetAsync(request.NoteId, request.PetId, Arg.Any<CancellationToken>());
        });
        await noteRepository.DidNotReceive().UpdateAsync(default);
    }
}
