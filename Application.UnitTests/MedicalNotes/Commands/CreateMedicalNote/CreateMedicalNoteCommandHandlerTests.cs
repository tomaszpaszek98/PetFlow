using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes.Commands.CreateMedicalNote;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

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
        var pet = new Pet
        {
            Id = command.PetId,
            Name = "Test Pet"
        };
        var createdNote = new MedicalNote
        {
            Id = 1,
            PetId = command.PetId,
            Title = command.Title,
            Description = command.Description,
            Created = DateTime.UtcNow
        };
        var repository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new CreateMedicalNoteCommandHandler(repository, petRepository);
        
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pet));
        repository.CreateAsync(
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
            petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
            repository.Received(1).CreateAsync(
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
        var repository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new CreateMedicalNoteCommandHandler(repository, petRepository);
        
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        await petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
        await repository.DidNotReceive().CreateAsync(default);
    }
}
