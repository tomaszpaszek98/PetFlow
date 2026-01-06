using Application.Events.Queries.GetEventDetails;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Events;

public class GetEventDetailsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnEventDetailsWhenEventExists()
    {
        // GIVEN
        var eventId = 1; // Event from seed data with assigned pets
        var query = new GetEventDetailsQuery { EventId = eventId };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Id.Should().Be(eventId);
        response.Title.Should().NotBeNullOrEmpty();
        response.AssignedPets.Should().NotBeNull();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var nonExistentEventId = 999999;
        var query = new GetEventDetailsQuery { EventId = nonExistentEventId };

        // WHEN
        var act = async () => await Sender.Send(query);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Event*{nonExistentEventId}*");
    }
}

