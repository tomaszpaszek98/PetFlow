using Application.Events.Commands.DeletePetFromEvent;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Events;

public class DeletePetFromEventTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldDeletePetFromEventWhenRelationshipExists()
    {
        // GIVEN
        var eventId = 1; // Event from seed data with assigned pet #1
        var petId = 1;
        var command = new DeletePetFromEventCommand
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
        var command = new DeletePetFromEventCommand
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
    public async Task ShouldThrowNotFoundExceptionWhenPetOrRelationshipDoesNotExist()
    {
        // GIVEN
        var eventId = 2;
        var nonExistentPetId = 999999;
        var command = new DeletePetFromEventCommand
        {
            EventId = eventId,
            PetId = nonExistentPetId
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
    }
}

