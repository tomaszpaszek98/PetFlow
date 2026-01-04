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
        var pet = new Pet
        {
            Id = query.PetId,
            Name = "Test Pet"
        };
        var note = new Note
        {
            Id = query.NoteId,
            PetId = query.PetId,
            Content = "Test Note Content",
            Type = Domain.Enums.NoteType.General,
            Created = DateTime.UtcNow.AddDays(-1),
            Pet = pet
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNoteDetailsQueryHandler(noteRepository, petRepository);
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(query.NoteId, query.PetId, Arg.Any<CancellationToken>())
            .Returns(note);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(note.Id);
        result.Content.Should().Be(note.Content);
        result.Type.Should().Be(note.Type);
        result.CreatedAt.Should().Be(note.Created);
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetByIdWithPetAsync(query.NoteId, query.PetId, Arg.Any<CancellationToken>());
        });
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
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNoteDetailsQueryHandler(noteRepository, petRepository);
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(query.NoteId, query.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Note>(null));
        
        // WHEN
        var act = async () => await handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Note)) && e.Message.Contains(query.NoteId.ToString()));
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetByIdWithPetAsync(query.NoteId, query.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotBelongToTheSpecifiedPet()
    {
        // GIVEN
        var query = new GetNoteDetailsQuery
        {
            PetId = 1,
            NoteId = 2
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNoteDetailsQueryHandler(noteRepository, petRepository);
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetByIdWithPetAsync(query.NoteId, query.PetId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Note>(null));
        
        // WHEN
        var act = async () => await handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetByIdWithPetAsync(query.NoteId, query.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var query = new GetNoteDetailsQuery
        {
            PetId = 999,
            NoteId = 1
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNoteDetailsQueryHandler(noteRepository, petRepository);
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(query.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().GetByIdWithPetAsync(default, default);
    }
}
