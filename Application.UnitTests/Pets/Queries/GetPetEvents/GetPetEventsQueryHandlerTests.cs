using Application.Pets.Queries.GetPetEvents;
using Domain.Entities;
using Application.Common.Interfaces.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Pets.Queries.GetPetEvents;

public class GetPetEventsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnAllEventsForPetWhenPetExistsAndHasEvents()
    {
        // GIVEN
        var petId = 5;
        var events = new List<Event>
        {
            new() { Id = 1, Title = "Vet", DateOfEvent = DateTime.Today.AddDays(1) },
            new() { Id = 2, Title = "Grooming", DateOfEvent = DateTime.Today.AddDays(2) }
        };
        var pet = new Pet
        {
            Id = petId,
            Name = "TestPet",
            Species = "Dog",
            Breed = "Labrador",
            DateOfBirth = new DateTime(2020, 1, 1),
            PhotoUrl = "url",
            Created = DateTime.Now.AddDays(-10),
            Modified = DateTime.Now.AddDays(-1),
            Events = events
        };
        var query = new GetPetEventsQuery { PetId = petId };
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetEventsQueryHandler(petRepository, Any.Instance<ILogger<GetPetEventsQueryHandler>>());
        
        petRepository.GetByIdWithEventsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        
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
        
        await petRepository.Received(1).GetByIdWithEventsAsync(petId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenPetDoesNotHaveEvents()
    {
        // GIVEN
        var petId = 10;
        var pet = new Pet
        {
            Id = petId,
            Name = "TestPet",
            Species = "Cat",
            Breed = "Siamese",
            DateOfBirth = new DateTime(2019, 5, 5),
            PhotoUrl = "caturl",
            Created = DateTime.Now.AddDays(-20),
            Modified = DateTime.Now.AddDays(-2),
            Events = new List<Event>()
        };
        var query = new GetPetEventsQuery { PetId = petId };
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetEventsQueryHandler(petRepository, Any.Instance<ILogger<GetPetEventsQueryHandler>>());
        
        petRepository.GetByIdWithEventsAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        
        await petRepository.Received(1).GetByIdWithEventsAsync(petId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var petId = 99;
        var query = new GetPetEventsQuery { PetId = petId };
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new GetPetEventsQueryHandler(petRepository, Any.Instance<ILogger<GetPetEventsQueryHandler>>());
        
        petRepository.GetByIdWithEventsAsync(petId, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(query, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(petId.ToString()));
        await petRepository.Received(1).GetByIdWithEventsAsync(petId, Arg.Any<CancellationToken>());
    }
}

