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
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
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
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenPetDoesNotHaveEvents()
    {
        // GIVEN
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var pet = new Pet
        {
            Id = 10,
            Name = "TestPet",
            PetEvents = new List<PetEvent>()
        };
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
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetEventsQueryHandler(petRepository, eventRepository);
        var query = new GetPetEventsQuery { PetId = 99 };
       
        petRepository.GetByIdAsync(99, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        eventRepository.GetEventsByPetIdAsync(99, Arg.Any<CancellationToken>())
            .Returns([]);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
