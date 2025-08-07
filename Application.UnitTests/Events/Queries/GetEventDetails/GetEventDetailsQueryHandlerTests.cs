using System.Collections.ObjectModel;
using Application.Events.Queries.GetEventDetails;
using Application.Common.Interfaces.Repositories;
using Application.Events.Common;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnEventDetailsWithAssignedPetsWhenEventAndPetsExist()
    {
        // GIVEN
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
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
            PetEvents = new List<PetEvent>
            {
                new() { PetId =  pets[0].Id },
                new() { PetId = pets[1].Id }
            }
        };
        var query = new GetEventDetailsQuery { EventId = eventId };
        var handler = new GetEventDetailsQueryHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.Contains(10) && ids.Contains(20)), Arg.Any<CancellationToken>())
            .Returns(pets);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(eventEntity.Title);
        result.Description.Should().Be(eventEntity.Description);
        result.DateOfEvent.Should().Be(eventEntity.DateOfEvent);
        result.AssignedPets.Should().HaveCount(2);
        result.AssignedPets.Should().BeEquivalentTo(expectedAssignedPets);
        
        Received.InOrder(() => {
            eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => 
                ids.Contains(pets[0].Id) && ids.Contains(pets[1].Id)), 
                Arg.Any<CancellationToken>());
        });
    }

    [Test]
    public async Task ShouldReturnEventDetailsWithEmptyAssignedPetsWhenNoPetEvents()
    {
        // GIVEN
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var eventId = 2;
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Event No Pets",
            Description = "No pets assigned",
            DateOfEvent = new DateTime(2025, 8, 3),
            Reminder = false,
            PetEvents = new List<PetEvent>()
        };
        var query = new GetEventDetailsQuery { EventId = eventId };
        var handler = new GetEventDetailsQueryHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(eventEntity.Title);
        result.Description.Should().Be(eventEntity.Description);
        result.DateOfEvent.Should().Be(eventEntity.DateOfEvent);
        result.AssignedPets.Should().BeEmpty();
        
        await eventRepository.Received(1).GetByIdAsync(eventId, Arg.Any<CancellationToken>());
        await petRepository.DidNotReceive().GetByIdsAsync(default, default);
    }

    [Test]
    public async Task ShouldReturnEventDetailsWithEmptyAssignedPetsWhenPetEventsIsNull()
    {
        // GIVEN
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var eventId = 3;
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Event Null Pets",
            Description = "Null pets",
            DateOfEvent = new DateTime(2025, 8, 3),
            Reminder = false,
            PetEvents = new Collection<PetEvent>()
        };
        var query = new GetEventDetailsQuery { EventId = eventId };
        var handler = new GetEventDetailsQueryHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(eventEntity.Title);
        result.Description.Should().Be(eventEntity.Description);
        result.DateOfEvent.Should().Be(eventEntity.DateOfEvent);
        result.AssignedPets.Should().BeEmpty();
        
        await eventRepository.Received(1).GetByIdAsync(eventId, Arg.Any<CancellationToken>());
        await petRepository.DidNotReceive().GetByIdsAsync(default, default);
    }

    [Test]
    public void ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var eventId = 99;
        var query = new GetEventDetailsQuery { EventId = eventId };
        var handler = new GetEventDetailsQueryHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns((Event)null);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        act.Should().ThrowAsync<NotFoundException>();
        eventRepository.Received(1).GetByIdAsync(eventId, Arg.Any<CancellationToken>());
        petRepository.DidNotReceive().GetByIdsAsync(default, default);
    }
}
