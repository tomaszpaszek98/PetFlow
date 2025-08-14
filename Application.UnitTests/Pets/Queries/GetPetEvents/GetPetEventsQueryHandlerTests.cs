using Application.Pets.Queries.GetPetEvents;
using Domain.Entities;
using Application.Common.Interfaces.Repositories;
using Domain.Exceptions;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;
using Persistance.Repositories;

namespace Application.UnitTests.Pets.Queries.GetPetEvents;

public class GetPetEventsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnAllEventsForPetWhenPetExistsAndHasEvents()
    {
        // GIVEN
        var events = new[]
        {
            new Event { Id = 1, Title = "Vet", DateOfEvent = DateTime.Today.AddDays(1) },
            new Event { Id = 2, Title = "Grooming", DateOfEvent = DateTime.Today.AddDays(2) }
        };
        var pet = new Pet
        {
            Id = 5,
            Name = "TestPet",
            PetEvents = events.Select(e => new PetEvent { Event = e }).ToList()
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetEventsQueryHandler(petRepository, eventRepository);
        var query = new GetPetEventsQuery { PetId = 5 };
        
        petRepository.GetByIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.GetEventsByPetIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(events);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        var items = result.Items.ToList();
        items.Should().HaveCount(2);
        items[0].Id.Should().Be(1);
        items[0].Title.Should().Be("Vet");
        items[1].Id.Should().Be(2);
        items[1].Title.Should().Be("Grooming");
        
        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(query.PetId, Arg.Any<CancellationToken>());
            eventRepository.Received(1).GetEventsByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenPetDoesNotHaveEvents()
    {
        // GIVEN
        var pet = new Pet
        {
            Id = 10,
            Name = "TestPet",
            PetEvents = new List<PetEvent>()
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetEventsQueryHandler(petRepository, eventRepository);
        var query = new GetPetEventsQuery { PetId = 10 };
        
        petRepository.GetByIdAsync(10, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.GetEventsByPetIdAsync(10, Arg.Any<CancellationToken>())
            .Returns([]);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        Received.InOrder(() =>
        {
            petRepository.Received(1).GetByIdAsync(query.PetId, Arg.Any<CancellationToken>());
            eventRepository.Received(1).GetEventsByPetIdAsync(query.PetId, Arg.Any<CancellationToken>());
        });
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var query = new GetPetEventsQuery { PetId = 99 };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetEventsQueryHandler(petRepository, eventRepository);
       
        petRepository.GetByIdAsync(99, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        eventRepository.GetEventsByPetIdAsync(99, Arg.Any<CancellationToken>())
            .Returns([]);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
        await petRepository.Received(1).GetByIdAsync(query.PetId, Arg.Any<CancellationToken>());
        await eventRepository.DidNotReceive().GetEventsByPetIdAsync(default);
    }
}
