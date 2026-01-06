using Application.Events.Commands.UpdateEvent;
using Domain.Exceptions;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Events;

public class UpdateEventTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldUpdateEventWhenEventExistsAndCommandIsValid()
    {
        // GIVEN
        var eventId = 1; // Event from seed data
        var updatedTitle = "Updated Event Title";
        var updatedDescription = "Updated description";
        var updateCommand = new UpdateEventCommand
        {
            Id = eventId,
            Title = updatedTitle,
            Description = updatedDescription,
            DateOfEvent = DateTime.UtcNow.Date.AddDays(45),
            Reminder = false,
            AssignedPetsIds = new[] { 1 }
        };

        // WHEN
        var result = await Sender.Send(updateCommand);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(updatedTitle);
        result.Description.Should().Be(updatedDescription);
        result.Reminder.Should().BeFalse();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var nonExistentEventId = 999999;
        var updateCommand = new UpdateEventCommand
        {
            Id = nonExistentEventId,
            Title = "Title",
            Description = "Description",
            DateOfEvent = DateTime.UtcNow.Date.AddDays(10),
            Reminder = true,
            AssignedPetsIds = new[] { 1 }
        };

        // WHEN
        var act = async () => await Sender.Send(updateCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*Event*{nonExistentEventId}*");
    }

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var eventId = 2; // Event from seed data
        var invalidUpdateCommand = new UpdateEventCommand
        {
            Id = eventId,
            Title = string.Empty,
            Description = "Description",
            DateOfEvent = DateTime.UtcNow.Date.AddDays(20),
            Reminder = false,
            AssignedPetsIds = new[] { 2 }
        };

        // WHEN
        var act = async () => await Sender.Send(invalidUpdateCommand);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }
}

