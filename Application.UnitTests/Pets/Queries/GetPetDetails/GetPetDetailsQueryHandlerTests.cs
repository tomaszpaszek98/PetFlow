using Application.Pets.Queries.GetPetDetails;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnPetDetailsResponseWhenPetExists()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var pet = new Pet
        {
            Id = 1,
            Name = "Denny",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = new DateTime(2020, 1, 1),
            PhotoUrl = "url",
            Created = DateTime.Now.AddDays(-10),
            Modified = DateTime.Now.AddDays(-1)
        };
        var handler = new GetPetDetailsQueryHandler(repository, eventRepository);
        var query = new GetPetDetailsQuery { PetId = 1 }; 
        repository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.GetEventsByPetIdAsync(1, Arg.Any<CancellationToken>())
            .Returns([]);

        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(pet.Id);
        result.Name.Should().Be(pet.Name);
        result.Species.Should().Be(pet.Species);
        result.Breed.Should().Be(pet.Breed);
        result.DateOfBirth.Should().Be(pet.DateOfBirth);
        result.PhotoUrl.Should().Be(pet.PhotoUrl);
        result.CreatedAt.Should().Be(pet.Created);
        result.ModifiedAt.Should().Be(pet.Modified);
        result.UpcomingEvent.Should().BeNull();
    }

    [Test]
    public async Task ShouldReturnPetDetailsResponseWithoutUpcomingEventWhenThereAreNoUpcomingEvents()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var pet = new Pet
        {
            Id = 2,
            Name = "Milo",
            Species = "Cat",
            Breed = "Siamese",
            DateOfBirth = new DateTime(2019, 5, 5),
            PhotoUrl = "caturl",
            Created = DateTime.Now.AddDays(-20),
            Modified = DateTime.Now.AddDays(-2)
        };
        var handler = new GetPetDetailsQueryHandler(repository, eventRepository);
        var query = new GetPetDetailsQuery { PetId = 2 };

        repository.GetByIdAsync(2, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.GetEventsByPetIdAsync(2, Arg.Any<CancellationToken>())
            .Returns([]);

        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(pet.Id);
        result.UpcomingEvent.Should().BeNull();
    }

    [Test]
    public async Task ShouldReturnPetDetailsResponseWithUpcomingEventWhenOneUpcomingEventExists()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var pet = new Pet
        {
            Id = 3,
            Name = "Rex",
            Species = "Dog",
            Breed = "Labrador",
            DateOfBirth = new DateTime(2018, 3, 3),
            PhotoUrl = "rexurl",
            Created = DateTime.Now.AddDays(-30),
            Modified = DateTime.Now.AddDays(-5)
        };
        var upcomingEvent = new Event
        {
            Id = 20,
            Title = "Vaccination",
            DateOfEvent = DateTime.Now.AddDays(2)
        };
        var handler = new GetPetDetailsQueryHandler(repository, eventRepository);
        var query = new GetPetDetailsQuery { PetId = 3 };

        repository.GetByIdAsync(3, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.GetEventsByPetIdAsync(3, Arg.Any<CancellationToken>())
            .Returns([upcomingEvent]);

        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(pet.Id);
        result.UpcomingEvent.Should().NotBeNull();
        result.UpcomingEvent.Id.Should().Be(upcomingEvent.Id);
        result.UpcomingEvent.Title.Should().Be(upcomingEvent.Title);
        result.UpcomingEvent.EventDate.Should().Be(upcomingEvent.DateOfEvent);
    }

    [Test]
    public async Task ShouldReturnPetDetailsResponseWithClosestUpcomingEventWhenMultipleUpcomingEventsExist()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var pet = new Pet
        {
            Id = 4,
            Name = "Bella",
            Species = "Dog",
            Breed = "Beagle",
            DateOfBirth = new DateTime(2017, 7, 7),
            PhotoUrl = "bellaurl",
            Created = DateTime.Now.AddDays(-40),
            Modified = DateTime.Now.AddDays(-10)
        };
        var anotherEvent = new Event
        {
            Id = 30,
            Title = "Grooming",
            DateOfEvent = DateTime.Now.AddDays(10)
        };
        var upcomingEvent = new Event
        {
            Id = 31,
            Title = "Vet Check",
            DateOfEvent = DateTime.Now.AddDays(3)
        };
        var handler = new GetPetDetailsQueryHandler(repository, eventRepository);
        var query = new GetPetDetailsQuery { PetId = 4 };

        repository.GetByIdAsync(4, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.GetEventsByPetIdAsync(4, Arg.Any<CancellationToken>())
            .Returns([anotherEvent, upcomingEvent]);

        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(pet.Id);
        result.UpcomingEvent.Should().NotBeNull();
        result.UpcomingEvent.Id.Should().Be(upcomingEvent.Id);
        result.UpcomingEvent.Title.Should().Be(upcomingEvent.Title);
        result.UpcomingEvent.EventDate.Should().Be(upcomingEvent.DateOfEvent);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new GetPetDetailsQueryHandler(repository, eventRepository);
        var query = new GetPetDetailsQuery { PetId = 99 };

        repository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns((Pet)null);

        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
    }
}