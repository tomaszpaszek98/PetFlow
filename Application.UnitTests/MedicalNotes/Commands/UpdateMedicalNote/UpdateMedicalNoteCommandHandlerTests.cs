using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes;
using Application.MedicalNotes.Commands.UpdateMedicalNote;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldUpdateMedicalNoteAndReturnResponseWhenAllConditionsAreMet()
    {
        // GIVEN
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 2,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var pet = new Pet { Id = command.PetId, Name = "Test Pet" };
        var existingNote = new MedicalNote
        {
            Id = command.MedicalNoteId,
            PetId = command.PetId,
            Title = "Original Title",
            Description = "Original Description",
            Created = DateTime.UtcNow.AddDays(-1)
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);
        
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(pet);
        medicalNoteRepository.GetByIdAsync(command.MedicalNoteId, Arg.Any<CancellationToken>())
            .Returns(existingNote);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(command.MedicalNoteId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);

        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.Received(1).GetByIdAsync(command.MedicalNoteId, Arg.Any<CancellationToken>());
            medicalNoteRepository.Received(1).UpdateAsync(
                Arg.Is<MedicalNote>(n => 
                    n.Id == command.MedicalNoteId && 
                    n.Title == command.Title && 
                    n.Description == command.Description), 
                Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 99,
            MedicalNoteId = 2,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);
        
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        await petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.DidNotReceive().GetByIdAsync(default);
        await medicalNoteRepository.DidNotReceive().UpdateAsync(default);
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotExist()
    {
        // GIVEN
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 99,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var pet = new Pet { Id = command.PetId, Name = "Test Pet" };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);
        
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(pet);
        medicalNoteRepository.GetByIdAsync(command.MedicalNoteId, Arg.Any<CancellationToken>())
            .Returns((MedicalNote)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(MedicalNote)) && e.Message.Contains(command.MedicalNoteId.ToString()));
        await petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.Received(1).GetByIdAsync(command.MedicalNoteId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.DidNotReceive().UpdateAsync(default);
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteBelongsToDifferentPet()
    {
        // GIVEN
        var petId = 1;
        var differentPetId = 999;
        var command = new UpdateMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = 2,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var pet = new Pet { Id = command.PetId, Name = "Test Pet" };
        var existingNote = new MedicalNote
        {
            Id = command.MedicalNoteId,
            PetId = differentPetId,
            Title = "Original Title",
            Description = "Original Description"
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);
        
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(pet);
        medicalNoteRepository.GetByIdAsync(command.MedicalNoteId, Arg.Any<CancellationToken>())
            .Returns(existingNote);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(command.MedicalNoteId.ToString()) && e.Message.Contains(command.PetId.ToString()));
        await petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.Received(1).GetByIdAsync(command.MedicalNoteId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.DidNotReceive().UpdateAsync(default);
    }
}
