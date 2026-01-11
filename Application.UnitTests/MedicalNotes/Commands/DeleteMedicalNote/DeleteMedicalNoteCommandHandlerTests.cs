using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes.Commands.DeleteMedicalNote;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldDeleteMedicalNoteSuccessfullyWhenMedicalNoteExistsAndBelongsToSpecificPet()
    {
        // GIVEN
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 1
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeleteMedicalNoteCommandHandler(medicalNoteRepository, petRepository, Any.Instance<ILogger<DeleteMedicalNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.DeleteAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);

        // THEN
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.DeleteAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotExist()
    {
        // GIVEN
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 999
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeleteMedicalNoteCommandHandler(medicalNoteRepository, petRepository, Any.Instance<ILogger<DeleteMedicalNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.DeleteAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(MedicalNote)) && e.Message.Contains(command.MedicalNoteId.ToString()));
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.DeleteAsync(command.MedicalNoteId, command.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 999,
            MedicalNoteId = 1
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeleteMedicalNoteCommandHandler(medicalNoteRepository, petRepository, Any.Instance<ILogger<DeleteMedicalNoteCommandHandler>>());
        
        petRepository.ExistsAsync(command.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(command.PetId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.DidNotReceive().DeleteAsync(default, default);
    }
}
