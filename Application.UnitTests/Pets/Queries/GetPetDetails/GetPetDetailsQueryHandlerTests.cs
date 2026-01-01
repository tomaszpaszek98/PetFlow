using Application.Pets.Queries.GetPetDetails;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnPetDetailsResponseWhenPetExistsWithoutUpcomingEvent()
    {
        // GIVEN
        var petId = 1;
        var pet = new Pet
        {
            Id = petId,
            Name = "Denny",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = new DateTime(2020, 1, 1),
            PhotoUrl = "url",
            Created = DateTime.Now.AddDays(-10),
            Modified = DateTime.Now.AddDays(-1),
            Events = new List<Event>()
        };
        var query = new GetPetDetailsQuery { PetId = petId };
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetDetailsQueryHandler(petRepository);
        
        petRepository.GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        
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
        
        await petRepository.Received(1).GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldReturnPetDetailsResponseWithUpcomingEventWhenEventExists()
    {
        // GIVEN
        var petId = 2;
        var upcomingEvent = new Event
        {
            Id = 20,
            Title = "Vaccination",
            DateOfEvent = DateTime.Now.AddDays(2)
        };
        var pet = new Pet
        {
            Id = petId,
            Name = "Milo",
            Species = "Cat",
            Breed = "Siamese",
            DateOfBirth = new DateTime(2019, 5, 5),
            PhotoUrl = "caturl",
            Created = DateTime.Now.AddDays(-20),
            Modified = DateTime.Now.AddDays(-2),
            Events = new List<Event> { upcomingEvent }
        };
        var query = new GetPetDetailsQuery { PetId = petId };
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetDetailsQueryHandler(petRepository);
        
        petRepository.GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(pet.Id);
        result.UpcomingEvent.Should().NotBeNull();
        result.UpcomingEvent.Id.Should().Be(upcomingEvent.Id);
        result.UpcomingEvent.Title.Should().Be(upcomingEvent.Title);
        result.UpcomingEvent.EventDate.Should().Be(upcomingEvent.DateOfEvent);
        
        await petRepository.Received(1).GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldReturnPetDetailsResponseWithUpcomingEventWhenMultipleEventsExist()
    {
        // GIVEN
        var petId = 3;
        var upcomingEvent = new Event
        {
            Id = 30,
            Title = "Vet Check",
            DateOfEvent = DateTime.Now.AddDays(3)
        };
        var pet = new Pet
        {
            Id = petId,
            Name = "Rex",
            Species = "Dog",
            Breed = "Labrador",
            DateOfBirth = new DateTime(2018, 3, 3),
            PhotoUrl = "rexurl",
            Created = DateTime.Now.AddDays(-30),
            Modified = DateTime.Now.AddDays(-5),
            Events = new List<Event> { upcomingEvent }
        };
        var query = new GetPetDetailsQuery { PetId = petId };
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetDetailsQueryHandler(petRepository);
        
        petRepository.GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(pet.Id);
        result.UpcomingEvent.Should().NotBeNull();
        result.UpcomingEvent.Id.Should().Be(upcomingEvent.Id);
        result.UpcomingEvent.Title.Should().Be(upcomingEvent.Title);
        result.UpcomingEvent.EventDate.Should().Be(upcomingEvent.DateOfEvent);
        
        await petRepository.Received(1).GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var petId = 99;
        var query = new GetPetDetailsQuery { PetId = petId };
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetDetailsQueryHandler(petRepository);
        
        petRepository.GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(petId.ToString()));
        await petRepository.Received(1).GetByIdWithUpcomingEventAsync(petId, Arg.Any<CancellationToken>());
    }
}

