using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes.Queries.GetMedicalNoteDetails;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.MedicalNotes.Queries.GetMedicalNoteDetails;

public class GetMedicalNoteDetailsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnMedicalNoteDetailsResponseWhenNoteExistsAndBelongsToPet()
    {
        // GIVEN
        var repository = Substitute.For<IMedicalNoteRepository>();
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = 1,
            MedicalNoteId = 2
        };
        var medicalNote = new MedicalNote
        {
            Id = query.MedicalNoteId,
            PetId = query.PetId,
            Title = "Test Medical Note",
            Description = "This is a test medical note description",
            Created = DateTime.UtcNow.AddDays(-1),
            Modified = DateTime.UtcNow
        };
        var handler = new GetMedicalNoteDetailsQueryHandler(repository);
        
        repository.GetByIdAsync(query.MedicalNoteId, Arg.Any<CancellationToken>())
            .Returns(medicalNote);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(medicalNote.Id);
        result.Title.Should().Be(medicalNote.Title);
        result.Description.Should().Be(medicalNote.Description);
        result.CreatedAt.Should().Be(medicalNote.Created);
        result.ModifiedAt.Should().Be(medicalNote.Modified.Value);
        await repository.Received(1).GetByIdAsync(query.MedicalNoteId, Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotExist()
    {
        // GIVEN
        var repository = Substitute.For<IMedicalNoteRepository>();
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = 1,
            MedicalNoteId = 99
        };
        var handler = new GetMedicalNoteDetailsQueryHandler(repository);
        
        repository.GetByIdAsync(query.MedicalNoteId, Arg.Any<CancellationToken>())
            .Returns((MedicalNote)null);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(MedicalNote)) && e.Message.Contains(query.MedicalNoteId.ToString()));
        await repository.Received(1).GetByIdAsync(query.MedicalNoteId, Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenMedicalNoteDoesNotBelongToPet()
    {
        // GIVEN
        var repository = Substitute.For<IMedicalNoteRepository>();
        var petId = 1;
        var differentPetId = 999;
        var query = new GetMedicalNoteDetailsQuery
        {
            PetId = petId,
            MedicalNoteId = 2
        };
        var medicalNote = new MedicalNote
        {
            Id = query.MedicalNoteId,
            PetId = differentPetId,
            Title = "Test Medical Note",
            Description = "This is a test medical note description",
            Created = DateTime.UtcNow
        };
        var handler = new GetMedicalNoteDetailsQueryHandler(repository);
        
        repository.GetByIdAsync(query.MedicalNoteId, Arg.Any<CancellationToken>())
            .Returns(medicalNote);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(query.MedicalNoteId.ToString()) && e.Message.Contains(query.PetId.ToString()));
        await repository.Received(1).GetByIdAsync(query.MedicalNoteId, Arg.Any<CancellationToken>());
    }
}
