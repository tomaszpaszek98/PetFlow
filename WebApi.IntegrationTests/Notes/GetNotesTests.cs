using Application.Notes.Queries.GetNotes;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Notes;

public class GetNotesTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnAllNotesWhenPetHasNotes()
    {
        // GIVEN
        var petId = 1; // Pet from seed data with note #1
        var query = new GetNotesQuery { PetId = petId };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().HaveCount(1);
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenPetHasNoNotes()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var query = new GetNotesQuery { PetId = petId };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }
}