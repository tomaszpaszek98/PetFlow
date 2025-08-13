using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes.Commands.DeleteMedicalNote;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.MedicalNotes.Commands.DeleteMedicalNote;

public class DeleteMedicalNoteCommandHandlerTests
{
    [Test]
    public async Task ShouldDeleteMedicalNoteSuccessfullyWhenMedicalNoteExistsAndBelongToSpecificPet()
    {
        // GIVEN
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 1
        };
        var medicalNote = new MedicalNote
        {
            Id = command.MedicalNoteId,
            PetId = command.PetId,
            Title = "Test Medical Note",
            Description = "This is a test medical note description"
        };
        var repository = Substitute.For<IMedicalNoteRepository>();
        var handler = new DeleteMedicalNoteCommandHandler(repository);
        
        repository.GetByIdAsync(command.MedicalNoteId)
            .Returns(Task.FromResult(medicalNote));
        
        // WHEN
        await handler.Handle(command, CancellationToken.None);

        // THEN
        await repository.Received(1).DeleteByIdAsync(command.MedicalNoteId, Arg.Any<CancellationToken>());
    }
    
    [Test]
    public Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotExist()
    {
        // GIVEN
        var command = new DeleteMedicalNoteCommand
        {
            PetId = 1,
            MedicalNoteId = 999
        };
        var repository = Substitute.For<IMedicalNoteRepository>();
        var handler = new DeleteMedicalNoteCommandHandler(repository);
        
        repository.GetByIdAsync(command.MedicalNoteId)
            .Returns(Task.FromResult<MedicalNote>(null));
        
        // WHEN/THEN
        var act = async () => await handler.Handle(command, CancellationToken.None);
        
        return FluentActions
            .Invoking(act)
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"MedicalNote with ID {command.MedicalNoteId} does not exist.");
    }
    
    [Test]
    public Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotBelongToPet()
    {
        // GIVEN
        var petId = 1;
        var differentPetId = 2;
        var command = new DeleteMedicalNoteCommand
        {
            PetId = petId,
            MedicalNoteId = 1
        };
        var medicalNote = new MedicalNote
        {
            Id = command.MedicalNoteId,
            PetId = differentPetId,
            Title = "Test Medical Note",
            Description = "This is a test medical note description"
        };
        var repository = Substitute.For<IMedicalNoteRepository>();
        var handler = new DeleteMedicalNoteCommandHandler(repository);
        
        repository.GetByIdAsync(command.MedicalNoteId)
            .Returns(Task.FromResult(medicalNote));
        
        // WHEN/THEN
        var act = async () => await handler.Handle(command, CancellationToken.None);
        
        return FluentActions
            .Invoking(act)
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Medical note with ID {command.MedicalNoteId} does not belong to pet with ID {command.PetId}");
    }
}
