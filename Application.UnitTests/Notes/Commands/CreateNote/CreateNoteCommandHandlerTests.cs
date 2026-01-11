using Application.Common.Interfaces.Repositories;
using Application.Notes.Commands.CreateNote;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Notes.Commands.CreateNote;

public class CreateNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnNoteResponseWhenNoteIsCreatedSuccessfully()
    {
        // GIVEN
        var command = new CreateNoteCommand
        {
            PetId = 1,
            Content = "Test Note Content",
            Type = Domain.Enums.NoteType.General
        };
        var expectedNote = new Note
        {
            Id = 1,
            PetId = command.PetId,
            Content = command.Content,
            Type = command.Type,
            Created = DateTime.UtcNow
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new CreateNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<CreateNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.CreateAsync(
                Arg.Is<Note>(n => n.PetId == command.PetId &&
                                 n.Content == command.Content &&
                                 n.Type == command.Type),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(expectedNote));
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedNote.Id);
        result.Content.Should().Be(command.Content);
        result.Type.Should().Be(command.Type);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            noteRepository.CreateAsync(
                Arg.Is<Note>(n => n.PetId == command.PetId &&
                                  n.Content == command.Content &&
                                  n.Type == command.Type),
                Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new CreateNoteCommand
        {
            PetId = 99,
            Content = "Test Note Content",
            Type = Domain.Enums.NoteType.General
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new CreateNoteCommandHandler(noteRepository, petRepository, Any.Instance<ILogger<CreateNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().CreateAsync(default);
    }
    
    [Test]
    public async Task ShouldLogSensitiveDetailsAtDebugLevelWhenHandlingCreateNoteCommand()
    {
        // GIVEN
        var petId = 1;
        var noteContent = "Sensitive Note Content With Private Info";
        var noteType = NoteType.General;
        var noteId = 10;
        var command = new CreateNoteCommand
        {
            PetId = petId,
            Content = noteContent,
            Type = noteType
        };
        var createdNote = new Note
        {
            Id = noteId,
            PetId = petId,
            Content = noteContent,
            Type = noteType,
            Created = DateTime.UtcNow
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<CreateNoteCommandHandler>>();
        var handler = new CreateNoteCommandHandler(noteRepository, petRepository, logger);
        
        petRepository.ExistsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.CreateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>())
            .Returns(createdNote);
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);
        
        // THEN
        logger.Received(1).Log(
            LogLevel.Debug,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(noteContent)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
    
    [Test]
    public async Task ShouldNotLogSensitiveDetailsAtInformationLevelWhenHandlingCreateNoteCommand()
    {
        // GIVEN
        var petId = 1;
        var noteContent = "Sensitive Note Content With Private Info";
        var noteType = NoteType.General;
        var noteId = 10;
        var command = new CreateNoteCommand
        {
            PetId = petId,
            Content = noteContent,
            Type = noteType
        };
        var createdNote = new Note
        {
            Id = noteId,
            PetId = petId,
            Content = noteContent,
            Type = noteType,
            Created = DateTime.UtcNow
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<CreateNoteCommandHandler>>();
        var handler = new CreateNoteCommandHandler(noteRepository, petRepository, logger);
        
        petRepository.ExistsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.CreateAsync(Arg.Any<Note>(), Arg.Any<CancellationToken>())
            .Returns(createdNote);
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);
        
        // THEN
        logger.DidNotReceive().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(noteContent)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
