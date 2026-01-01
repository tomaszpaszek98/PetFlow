using Application.Events.Queries.GetEventDetails;
using Application.Common.Interfaces.Repositories;
using Application.Events.Common;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnEventDetailsWithAssignedPetsWhenEventAndPetsExist()
    {
        // GIVEN
        var eventId = 1;
        var pets = new List<Pet>
        {
            new() { Id = 10, Name = "Rex", PhotoUrl = "https://example.com/rex.jpg" },
            new() { Id = 20, Name = "Milo", PhotoUrl = "https://example.com/milo.jpg" }
        };
        var expectedAssignedPets = new List<AssignedPetDto>
        {
            new() { Id = 10, Name = "Rex", PhotoUrl = "https://example.com/rex.jpg" },
            new() { Id = 20, Name = "Milo", PhotoUrl = "https://example.com/milo.jpg" }
        };
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Test Event",
            Description = "Desc",
            DateOfEvent = new DateTime(2025, 8, 3),
            Reminder = true,
            Pets = pets
        };
        var query = new GetEventDetailsQuery { EventId = eventId };
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new GetEventDetailsQueryHandler(eventRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(eventEntity.Title);
        result.Description.Should().Be(eventEntity.Description);
        result.DateOfEvent.Should().Be(eventEntity.DateOfEvent);
        result.Reminder.Should().Be(eventEntity.Reminder);
        result.AssignedPets.Should().HaveCount(2);
        result.AssignedPets.Should().BeEquivalentTo(expectedAssignedPets);
        
        await eventRepository.Received(1).GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldReturnEventDetailsWithEmptyAssignedPetsWhenNoPetsAssigned()
    {
        // GIVEN
        var eventId = 2;
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Event No Pets",
            Description = "No pets assigned",
            DateOfEvent = new DateTime(2025, 8, 3),
            Reminder = false,
            Pets = new List<Pet>()
        };
        var query = new GetEventDetailsQuery { EventId = eventId };
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new GetEventDetailsQueryHandler(eventRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(eventEntity.Title);
        result.Description.Should().Be(eventEntity.Description);
        result.DateOfEvent.Should().Be(eventEntity.DateOfEvent);
        result.Reminder.Should().Be(eventEntity.Reminder);
        result.AssignedPets.Should().BeEmpty();
        
        await eventRepository.Received(1).GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var eventId = 99;
        var query = new GetEventDetailsQuery { EventId = eventId };
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new GetEventDetailsQueryHandler(eventRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns((Event)null);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Event)) && e.Message.Contains(eventId.ToString()));
        await eventRepository.Received(1).GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
    }
}