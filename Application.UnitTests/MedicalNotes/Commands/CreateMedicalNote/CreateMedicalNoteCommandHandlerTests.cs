using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes.Commands.CreateMedicalNote;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.MedicalNotes.Commands.CreateMedicalNote;

public class CreateMedicalNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnMedicalNoteResponseWhenNoteIsCreatedSuccessfully()
    {
        // GIVEN
        var command = new CreateMedicalNoteCommand
        {
            PetId = 1,
            Title = "Test Medical Note",
            Description = "This is a test medical note description"
        };
        var createdNote = new MedicalNote
        {
            Id = 1,
            PetId = command.PetId,
            Title = command.Title,
            Description = command.Description,
            Created = DateTime.UtcNow
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new CreateMedicalNoteCommandHandler(medicalNoteRepository, petRepository, Any.Instance<ILogger<CreateMedicalNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.CreateAsync(
                Arg.Is<MedicalNote>(n => n.PetId == command.PetId &&
                                       n.Title == command.Title &&
                                       n.Description == command.Description),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(createdNote));
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(createdNote.Id);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.CreateAsync(
                Arg.Is<MedicalNote>(n => n.PetId == command.PetId &&
                                         n.Title == command.Title &&
                                         n.Description == command.Description),
                Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new CreateMedicalNoteCommand
        {
            PetId = 99,
            Title = "Test Medical Note",
            Description = "This is a test medical note description"
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new CreateMedicalNoteCommandHandler(medicalNoteRepository, petRepository, Any.Instance<ILogger<CreateMedicalNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.DidNotReceive().CreateAsync(default);
    }
    
    [Test]
    public async Task ShouldLogSensitiveDetailsAtDebugLevelWhenHandlingCreateMedicalNoteCommand()
    {
        // GIVEN
        var petId = 1;
        var title = "Sensitive Medical Title";
        var description = "Sensitive Medical Description With Private Info";
        var medicalNoteId = 10;
        var command = new CreateMedicalNoteCommand
        {
            PetId = petId,
            Title = title,
            Description = description
        };
        var createdNote = new MedicalNote
        {
            Id = medicalNoteId,
            PetId = petId,
            Title = title,
            Description = description,
            Created = DateTime.UtcNow
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<CreateMedicalNoteCommandHandler>>();
        var handler = new CreateMedicalNoteCommandHandler(medicalNoteRepository, petRepository, logger);
        
        petRepository.ExistsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.CreateAsync(Arg.Any<MedicalNote>(), Arg.Any<CancellationToken>())
            .Returns(createdNote);
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);
        
        // THEN
        logger.Received(1).Log(
            LogLevel.Debug,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(description)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
    
    [Test]
    public async Task ShouldNotLogSensitiveDetailsAtInformationLevelWhenHandlingCreateMedicalNoteCommand()
    {
        // GIVEN
        var petId = 1;
        var title = "Sensitive Medical Title";
        var description = "Sensitive Medical Description With Private Info";
        var medicalNoteId = 10;
        var command = new CreateMedicalNoteCommand
        {
            PetId = petId,
            Title = title,
            Description = description
        };
        var createdNote = new MedicalNote
        {
            Id = medicalNoteId,
            PetId = petId,
            Title = title,
            Description = description,
            Created = DateTime.UtcNow
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<CreateMedicalNoteCommandHandler>>();
        var handler = new CreateMedicalNoteCommandHandler(medicalNoteRepository, petRepository, logger);
        
        petRepository.ExistsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.CreateAsync(Arg.Any<MedicalNote>(), Arg.Any<CancellationToken>())
            .Returns(createdNote);
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);
        
        // THEN
        logger.DidNotReceive().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(description)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
