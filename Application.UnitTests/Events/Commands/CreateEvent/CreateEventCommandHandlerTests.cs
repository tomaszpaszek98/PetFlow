using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.CreateEvent;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;

namespace Application.UnitTests.Events.Commands.CreateEvent;

public class CreateEventCommandHandlerTests
{
    [Test]
    public async Task ShouldCreateEventAndReturnCreateEventResponseWithAssignedPetsWhenAllPetsExist()
    {
        // GIVEN
        var petIds = new List<int> { 1, 2 };
        var eventId = 10;
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Event Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = petIds
        };
        var pets = new List<Pet>
        {
            new() { Id = 1, Name = "Rex", PhotoUrl = "https://example.com/rex.jpg" },
            new() { Id = 2, Name = "Milo", PhotoUrl = "https://example.com/milo.jpg" }
        };
        var petEvents = pets.Select(p => new PetEvent { PetId = p.Id, Pet = p }).ToList();
        var createdEvent = new Event
        {
            Id = eventId,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = petEvents
        };
        
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new CreateEventCommandHandler(petRepository, eventRepository);
        
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(petIds)), Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.CreateAsync(
                Arg.Is<Event>(e => 
                    e.Title == command.Title && 
                    e.Description == command.Description && 
                    e.DateOfEvent == command.DateOfEvent &&
                    e.Reminder == command.Reminder &&
                    e.PetEvents.Count == 2), 
                Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DateOfEvent.Should().Be(command.DateOfEvent);
        result.Reminder.Should().Be(command.Reminder);
        
        Received.InOrder(() => {
            petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(petIds)), Arg.Any<CancellationToken>());
            eventRepository.CreateAsync(Arg.Is<Event>(e => 
                e.Title == command.Title && 
                e.Description == command.Description && 
                e.DateOfEvent == command.DateOfEvent &&
                e.Reminder == command.Reminder &&
                e.PetEvents.Count == 2), Arg.Any<CancellationToken>());
        });
    }

    [Test]
    public async Task ShouldCreateEventAndReturnCreateEventResponseWithoutPetsWhenNoPetIdsProvided()
    {
        // GIVEN
        var eventId = 11;
        var emptyPetIds = new List<int>();
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Event Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = emptyPetIds
        };
        var createdEvent = new Event
        {
            Id = eventId,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = new List<PetEvent>()
        };
        
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new CreateEventCommandHandler(petRepository, eventRepository);
        
        eventRepository.CreateAsync(
                Arg.Is<Event>(e => 
                    e.Title == command.Title && 
                    e.Description == command.Description && 
                    e.DateOfEvent == command.DateOfEvent &&
                    e.Reminder == command.Reminder &&
                    e.PetEvents.Count == 0),
                Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DateOfEvent.Should().Be(command.DateOfEvent);
        result.Reminder.Should().Be(command.Reminder);
        
        await petRepository.DidNotReceive().GetByIdsAsync(default);
        await eventRepository.Received(1).CreateAsync(
            Arg.Is<Event>(e => 
                e.Title == command.Title && 
                e.Description == command.Description && 
                e.DateOfEvent == command.DateOfEvent &&
                e.Reminder == command.Reminder &&
                e.PetEvents.Count == 0),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldCreateEventAndReturnCreateEventResponseWithoutPetsWhenPetToAssignIdsIsNull()
    {
        // GIVEN
        var eventId = 12;
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Event Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = null
        };
        var createdEvent = new Event
        {
            Id = eventId,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = new List<PetEvent>()
        };
        
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new CreateEventCommandHandler(petRepository, eventRepository);
        
        eventRepository.CreateAsync(
                Arg.Is<Event>(e => 
                    e.Title == command.Title && 
                    e.Description == command.Description && 
                    e.DateOfEvent == command.DateOfEvent &&
                    e.Reminder == command.Reminder &&
                    e.PetEvents.Count == 0),
                Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DateOfEvent.Should().Be(command.DateOfEvent);
        result.Reminder.Should().Be(command.Reminder);
        
        await petRepository.DidNotReceive().GetByIdsAsync(default);
        await eventRepository.Received(1).CreateAsync(
            Arg.Is<Event>(e => 
                e.Title == command.Title && 
                e.Description == command.Description && 
                e.DateOfEvent == command.DateOfEvent &&
                e.Reminder == command.Reminder &&
                e.PetEvents.Count == 0),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenSomePetsDoNotExist()
    {
        // GIVEN
        var requestedPetIds = new List<int> { 1, 2, 999 };
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Event Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = requestedPetIds
        };
        var foundPets = new List<Pet>
        {
            new() { Id = 1, Name = "Rex", PhotoUrl = "https://example.com/rex.jpg" },
            new() { Id = 2, Name = "Milo", PhotoUrl = "https://example.com/milo.jpg" }
        };
        
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new CreateEventCommandHandler(petRepository, eventRepository);
        
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>())
            .Returns(foundPets);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains("999"));
        
        await petRepository.Received(1).GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>());
        await eventRepository.DidNotReceive().CreateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenAllPetsDoNotExist()
    {
        // GIVEN
        var requestedPetIds = new List<int> { 999, 998, 997 };
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Event Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = requestedPetIds
        };
        var foundPets = new List<Pet>();
        
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new CreateEventCommandHandler(petRepository, eventRepository);
        
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>())
            .Returns(foundPets);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains("Pets not found"));
        
        await petRepository.Received(1).GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>());
        await eventRepository.DidNotReceive().CreateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldRemoveDuplicatePetIdsBeforeAssigningWhenPetToAssignIdsContainsDuplicates()
    {
        // GIVEN
        var duplicatePetIds = new List<int> { 1, 1, 2, 2, 3 };
        var uniquePetIds = new List<int> { 1, 2, 3 };
        var eventId = 10;
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Event Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = duplicatePetIds
        };
        var pets = new List<Pet>
        {
            new() { Id = 1, Name = "Pet1", PhotoUrl = "url1" },
            new() { Id = 2, Name = "Pet2", PhotoUrl = "url2" },
            new() { Id = 3, Name = "Pet3", PhotoUrl = "url3" }
        };
        var petEvents = pets.Select(p => new PetEvent { PetId = p.Id, Pet = p }).ToList();
        var createdEvent = new Event
        {
            Id = eventId,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = petEvents
        };
        
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new CreateEventCommandHandler(petRepository, eventRepository);
        
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(uniquePetIds)), Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.CreateAsync(
                Arg.Is<Event>(e => 
                    e.Title == command.Title && 
                    e.Description == command.Description && 
                    e.DateOfEvent == command.DateOfEvent &&
                    e.Reminder == command.Reminder &&
                    e.PetEvents.Count == 3),
                Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(command.Title);

        await petRepository.Received(1).GetByIdsAsync(
            Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(uniquePetIds)),
            Arg.Any<CancellationToken>());
    }
}

