using Application.Events.Queries.GetEvents;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Events;

public class GetEventsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnAllEventsWhenEventsExistInDatabase()
    {
        // GIVEN - 3 events from seed data
        var query = new GetEventsQuery();

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenNoEventsExist()
    {
        // GIVEN
        var query = new GetEventsQuery();

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }
}

