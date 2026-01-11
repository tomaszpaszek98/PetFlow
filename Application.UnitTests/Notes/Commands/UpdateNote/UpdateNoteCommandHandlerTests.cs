using Application.Common.Interfaces.Repositories;
using Application.Notes.Commands.UpdateNote;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Notes.Commands.UpdateNote;

public class UpdateNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldUpdateNoteAndReturnUpdatedNoteResponseWhenNoteExists()
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
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<UpdateNoteCommandHandler>>());
        
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
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<UpdateNoteCommandHandler>>());
        
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
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<UpdateNoteCommandHandler>>());
        
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
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<UpdateNoteCommandHandler>>());
        
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
    
    [Test]
    public async Task ShouldLogSensitiveDetailsAtDebugLevelWhenHandlingUpdateNoteCommand()
    {
        // GIVEN
        var petId = 1;
        var noteId = 1;
        var noteContent = "Sensitive Updated Note Content";
        var noteType = NoteType.General;
        var request = new UpdateNoteCommand
        {
            PetId = petId,
            NoteId = noteId,
            Content = noteContent,
            Type = noteType
        };
        var existingNote = new Note
        {
            Id = noteId,
            PetId = petId,
            Content = "Old Content",
            Type = NoteType.Behaviour,
            Created = DateTime.UtcNow.AddSeconds(-10)
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<UpdateNoteCommandHandler>>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository, logger);
        
        petRepository.ExistsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(noteId, petId, Arg.Any<CancellationToken>())
            .Returns(existingNote);
        noteRepository.UpdateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        await handler.Handle(request, CancellationToken.None);
        
        // THEN
        logger.Received(1).Log(
            LogLevel.Debug,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(noteContent)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
    
    [Test]
    public async Task ShouldNotLogSensitiveDetailsAtInformationLevelWhenHandlingUpdateNoteCommand()
    {
        // GIVEN
        var petId = 1;
        var noteId = 1;
        var noteContent = "Sensitive Updated Note Content";
        var noteType = NoteType.General;
        var request = new UpdateNoteCommand
        {
            PetId = petId,
            NoteId = noteId,
            Content = noteContent,
            Type = noteType
        };
        var existingNote = new Note
        {
            Id = noteId,
            PetId = petId,
            Content = "Old Content",
            Type = NoteType.Behaviour,
            Created = DateTime.UtcNow.AddSeconds(-10)
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<UpdateNoteCommandHandler>>();
        var handler = new UpdateNoteCommandHandler(noteRepository, petRepository, logger);
        
        petRepository.ExistsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(noteId, petId, Arg.Any<CancellationToken>())
            .Returns(existingNote);
        noteRepository.UpdateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        await handler.Handle(request, CancellationToken.None);
        
        // THEN
        logger.DidNotReceive().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(noteContent)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
