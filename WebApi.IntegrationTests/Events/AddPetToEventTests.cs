using Application.Events.Commands.AddPetToEvent;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Events;

public class AddPetToEventTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldAddPetToEventWhenBothExist()
    {
        // GIVEN
        var eventId = 2; // Event from seed data
        var petId = 3; // Pet from seed data (not yet assigned to event #2)
        var command = new AddPetToEventCommand
        {
            EventId = eventId,
            PetId = petId
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var nonExistentEventId = 999999;
        var petId = 1;
        var command = new AddPetToEventCommand
        {
            EventId = nonExistentEventId,
            PetId = petId
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Event*{nonExistentEventId}*");
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var eventId = 1;
        var nonExistentPetId = 999999;
        var command = new AddPetToEventCommand
        {
            EventId = eventId,
            PetId = nonExistentPetId
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Pet*{nonExistentPetId}*");
    }
}

