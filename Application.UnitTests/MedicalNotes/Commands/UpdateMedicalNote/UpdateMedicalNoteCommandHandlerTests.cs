using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes;
using Application.MedicalNotes.Commands.UpdateMedicalNote;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.MedicalNotes.Commands.UpdateMedicalNote;

public class UpdateMedicalNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnMedicalNoteResponseWhenMedicalNoteIsUpdatedSuccessfully()
    {
        // GIVEN
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 1,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var pet = new Pet
        {
            Id = command.PetId,
            Name = "Test Pet"
        };
        var medicalNote = new MedicalNote
        {
            Id = command.MedicalNoteId,
            PetId = command.PetId,
            Title = "Old Title",
            Description = "Old Description",
            Pet = pet
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);

        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.GetByIdWithPetAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(medicalNote));
        medicalNoteRepository.UpdateAsync(Arg.Any<MedicalNote>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(command.MedicalNoteId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.GetByIdWithPetAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.UpdateAsync(Arg.Is<MedicalNote>(m => 
                m.Id == command.MedicalNoteId && 
                m.Title == command.Title && 
                m.Description == command.Description), 
                Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotExist()
    {
        // GIVEN
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 999,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);

        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.GetByIdWithPetAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<MedicalNote>(null));
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(MedicalNote)) && e.Message.Contains(command.MedicalNoteId.ToString()));
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.GetByIdWithPetAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>());
        });
        await medicalNoteRepository.DidNotReceive().UpdateAsync(default);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotBelongToPet()
    {
        // GIVEN
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 1,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);

        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.GetByIdWithPetAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<MedicalNote>(null));
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.GetByIdWithPetAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>());
        });
        await medicalNoteRepository.DidNotReceive().UpdateAsync(default);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new UpdateMedicalNoteCommand
        {
            PetId = 999,
            MedicalNoteId = 1,
            Title = "Updated Title",
            Description = "Updated Description"
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new UpdateMedicalNoteCommandHandler(medicalNoteRepository, petRepository);

        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.DidNotReceive().GetByIdWithPetAsync(default, default);
        await medicalNoteRepository.DidNotReceive().UpdateAsync(default);
    }
}
