using Application.Common.Interfaces.Repositories;
using Application.Notes.Queries.GetNoteDetails;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnNoteDetailsResponseWhenNoteExistsAndBelongsToPet()
    {
        // GIVEN
        var query = new GetNoteDetailsQuery
        {
            PetId = 1,
            NoteId = 2
        };
        var note = new Note
        {
            Id = query.NoteId,
            PetId = query.PetId,
            Content = "Test Note Content",
            Type = Domain.Enums.NoteType.General,
            Created = DateTime.UtcNow.AddDays(-1)
        };
        var repository = Substitute.For<INoteRepository>();
        var handler = new GetNoteDetailsQueryHandler(repository);
        
        repository.GetByIdAsync(query.NoteId, Arg.Any<CancellationToken>())
            .Returns(note);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(note.Id);
        result.Content.Should().Be(note.Content);
        result.Type.Should().Be(note.Type);
        result.CreatedAt.Should().Be(note.Created);
        await repository.Received(1).GetByIdAsync(query.NoteId, Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var query = new GetNoteDetailsQuery
        {
            PetId = 1,
            NoteId = 99
        };
        var repository = Substitute.For<INoteRepository>();
        var handler = new GetNoteDetailsQueryHandler(repository);
        
        repository.GetByIdAsync(query.NoteId, Arg.Any<CancellationToken>())
            .Returns((Note)null);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Note)) && e.Message.Contains(query.NoteId.ToString()));
        await repository.Received(1).GetByIdAsync(query.NoteId, Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotBelongToPet()
    {
        // GIVEN
        var query = new GetNoteDetailsQuery
        {
            PetId = 1,
            NoteId = 2
        };
        var note = new Note
        {
            Id = query.NoteId,
            PetId = 3, // Different pet ID
            Content = "Test Note Content",
            Type = Domain.Enums.NoteType.General,
            Created = DateTime.UtcNow.AddDays(-1)
        };
        var repository = Substitute.For<INoteRepository>();
        var handler = new GetNoteDetailsQueryHandler(repository);
        
        repository.GetByIdAsync(query.NoteId, Arg.Any<CancellationToken>())
            .Returns(note);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains($"Note with ID {query.NoteId}") && 
                   e.Message.Contains($"does not belong to pet with ID {query.PetId}"));
        await repository.Received(1).GetByIdAsync(query.NoteId, Arg.Any<CancellationToken>());
    }
}
