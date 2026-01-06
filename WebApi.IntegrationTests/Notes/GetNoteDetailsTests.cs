using Application.Notes.Queries.GetNoteDetails;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Notes;

public class GetNoteDetailsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnNoteDetailsWhenNoteExists()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var noteId = 1; // Note from seed data for pet #1
        var query = new GetNoteDetailsQuery
        {
            PetId = petId,
            NoteId = noteId
        };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Id.Should().Be(noteId);
        response.Content.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoteDoesNotExist()
    {
        // GIVEN
        var petId = 1;
        var nonExistentNoteId = 999999;
        var query = new GetNoteDetailsQuery
        {
            PetId = petId,
            NoteId = nonExistentNoteId
        };

        // WHEN
        var act = async () => await Sender.Send(query);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Note*{nonExistentNoteId}*");
    }
}

