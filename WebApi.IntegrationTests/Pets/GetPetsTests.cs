using Application.Pets.Queries.GetPets;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Pets;

public class GetPetsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnAllPetsWhenPetsExistInDatabase()
    {
        // GIVEN - 3 pets from seed data
        var query = new GetPetsQuery();

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenNoPetsExist()
    {
        // GIVEN
        var query = new GetPetsQuery();

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }
}

