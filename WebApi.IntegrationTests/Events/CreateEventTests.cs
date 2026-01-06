using Application.Events.Commands.CreateEvent;
using Domain.Exceptions;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Events;

public class CreateEventTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldCreateEventWhenCommandIsValid()
    {
        // GIVEN
        var title = "New Event";
        var description = "Test event description";
        var command = new CreateEventCommand
        {
            Title = title,
            Description = description,
            DateOfEvent = DateTime.UtcNow.Date.AddDays(30),
            Reminder = true
        };

        // WHEN
        var result = await Sender.Send(command);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be(title);
        result.Description.Should().Be(description);
        result.Reminder.Should().BeTrue();
    }

    [Test]
    public async Task ShouldCreateEventWithAssignedPetsWhenPetIdsAreProvided()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "Event with Pets",
            Description = "Event assigned to pets",
            DateOfEvent = DateTime.UtcNow.Date.AddDays(15),
            Reminder = false,
            PetToAssignIds = new[] { 1, 2 } // Pets from seed data
        };

        // WHEN
        var result = await Sender.Send(command);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.AssignedPets.Should().HaveCount(2);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetIdDoesNotExist()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = "Event",
            Description = "Event with non-existent pet",
            DateOfEvent = DateTime.UtcNow.Date.AddDays(10),
            Reminder = true,
            PetToAssignIds = new[] { 999999 }
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*999999*");
    }

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var command = new CreateEventCommand
        {
            Title = string.Empty,
            Description = "Description",
            DateOfEvent = DateTime.UtcNow.Date.AddDays(5),
            Reminder = false
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }
}

