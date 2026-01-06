using Application.Common.Interfaces.Repositories;
using Application.MedicalNotes;
using Application.MedicalNotes.Queries.GetMedicalNotes;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.MedicalNotes.Queries.GetMedicalNotes;

public class GetMedicalNotesQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnMedicalNotesResponseWhenPetExists()
    {
        // GIVEN
        var query = new GetMedicalNotesQuery { PetId = 1 };
        var medicalNotes = new List<MedicalNote>
        {
            new() { Id = 1, PetId = query.PetId, Title = "Note 1", Created = DateTime.UtcNow.AddDays(-1) },
            new() { Id = 2, PetId = query.PetId, Title = "Note 2", Created = DateTime.UtcNow }
        };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetMedicalNotesQueryHandler(medicalNoteRepository, petRepository);
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(medicalNotes);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        
        Received.InOrder(() =>
        {
            petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
            medicalNoteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var query = new GetMedicalNotesQuery { PetId = 99 };
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetMedicalNotesQueryHandler(medicalNoteRepository, petRepository);
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(query.PetId.ToString()));
        
        await petRepository.Received(1).ExistsAsync(query.PetId, Arg.Any<CancellationToken>());
        await medicalNoteRepository.DidNotReceive().GetAllByPetIdAsync(default);
    }
    
    [Test]
    public async Task ShouldReturnEmptyResponseWhenPetHasNoMedicalNotes()
    {
        // GIVEN
        var query = new GetMedicalNotesQuery { PetId = 1 };
        var emptyNotes = new List<MedicalNote>();
        var medicalNoteRepository = Substitute.For<IMedicalNoteRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetMedicalNotesQueryHandler(medicalNoteRepository, petRepository);
        
        petRepository.ExistsAsync(query.PetId, Arg.Any<CancellationToken>())
            .Returns(true);
        medicalNoteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>())
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
            medicalNoteRepository.GetAllByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }
}
