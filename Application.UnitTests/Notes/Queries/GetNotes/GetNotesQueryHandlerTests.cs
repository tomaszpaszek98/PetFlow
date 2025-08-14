using Application.Common.Interfaces.Repositories;
using Application.Notes.Queries.GetNotes;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Notes.Queries.GetNotes;

public class GetNotesQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnNotesResponseWhenPetExists()
    {
        // GIVEN
        var query = new GetNotesQuery { PetId = 1 };
        var pet = new Pet { Id = query.PetId, Name = "Test Pet" };
        var notes = new List<Note>
        {
            new() { Id = 1, PetId = query.PetId, Content = "Note 1 content", Created = DateTime.UtcNow.AddDays(-1) },
            new() { Id = 2, PetId = query.PetId, Content = "Note 2 content", Created = DateTime.UtcNow }
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNotesQueryHandler(noteRepository, petRepository);
        
        petRepository.GetByIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(pet);
        noteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(notes);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Notes.Should().NotBeNull();
        result.Notes.Should().HaveCount(2);
        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(query.PetId, Arg.Any<CancellationToken>());
            noteRepository.Received(1).GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var query = new GetNotesQuery { PetId = 99 };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNotesQueryHandler(noteRepository, petRepository);
        
        petRepository.GetByIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(query.PetId.ToString()));
        await petRepository.Received(1).GetByIdAsync(query.PetId, Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().GetAllByPetIdAsync(default, default);
    }
    
    [Test]
    public async Task ShouldReturnEmptyResponseWhenPetHasNoNotes()
    {
        // GIVEN
        var query = new GetNotesQuery { PetId = 1 };
        var pet = new Pet { Id = query.PetId, Name = "Test Pet" };
        var emptyNotes = new List<Note>();
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNotesQueryHandler(noteRepository, petRepository);
        
        petRepository.GetByIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(pet);
        noteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(emptyNotes);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Notes.Should().NotBeNull();
        result.Notes.Should().BeEmpty();
        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(query.PetId, Arg.Any<CancellationToken>());
            noteRepository.Received(1).GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }
}
