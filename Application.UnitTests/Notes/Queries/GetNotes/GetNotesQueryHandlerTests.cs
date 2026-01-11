using Application.Common.Interfaces.Repositories;
using Application.Notes.Queries.GetNotes;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Notes.Queries.GetNotes;

public class GetNotesQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnNotesResponseWithAllNotesWhenPetExists()
    {
        // GIVEN
        var query = new GetNotesQuery { PetId = 1 };
        var notes = new List<Note>
        {
            new() { Id = 1, PetId = query.PetId, Content = "Note 1 content", Created = DateTime.UtcNow.AddDays(-1) },
            new() { Id = 2, PetId = query.PetId, Content = "Note 2 content", Created = DateTime.UtcNow }
        };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNotesQueryHandler(noteRepository, petRepository, Any.Instance<ILogger<GetNotesQueryHandler>>());
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(notes);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var query = new GetNotesQuery { PetId = 99 };
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNotesQueryHandler(noteRepository, petRepository, Any.Instance<ILogger<GetNotesQueryHandler>>());
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = async () => await handler.Handle(query, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(query.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
        await noteRepository.DidNotReceive().GetAllByPetIdAsync(default);
    }
    
    [Test]
    public async Task ShouldReturnEmptyResponseWhenPetHasNoNotes()
    {
        // GIVEN
        var query = new GetNotesQuery { PetId = 1 };
        var emptyNotes = new List<Note>();
        var noteRepository = Substitute.For<INoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetNotesQueryHandler(noteRepository, petRepository, Any.Instance<ILogger<GetNotesQueryHandler>>());
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        noteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(emptyNotes);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
            noteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }
}
