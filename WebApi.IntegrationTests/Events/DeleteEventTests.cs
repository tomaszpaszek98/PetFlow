using Application.Events.Commands.DeleteEvent;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Events;

public class DeleteEventTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldDeleteEventWhenEventExists()
    {
        // GIVEN
        var eventId = 1; // Event from seed data
        var deleteCommand = new DeleteEventCommand { EventId = eventId };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var nonExistentEventId = 999999;
        var deleteCommand = new DeleteEventCommand { EventId = nonExistentEventId };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Event*{nonExistentEventId}*");
    }
}

