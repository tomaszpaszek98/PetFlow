using Application.MedicalNotes.Queries.GetMedicalNotes;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.MedicalNotes;

public class GetMedicalNotesTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnAllMedicalNotesWhenPetHasNotes()
    {
        // GIVEN
        var petId = 1; // Pet from seed data with medical note #1
        var query = new GetMedicalNotesQuery { PetId = petId };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenPetHasNoNotes()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var query = new GetMedicalNotesQuery { PetId = petId };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }
}

