using Application.Events.Commands.CreateEvent;
using Application.Common.Interfaces.Repositories;
using Application.Events.Common;
using Domain.Entities;
using Persistance.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Events.Commands.CreateEvent;

public class CreateEventCommandHandlerTests
{
    [Test]
    public async Task ShouldCreateEventAssignPetAndReturnResponseWithoutMissingIdsWhenAllPetsExists()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<CreateEventCommandHandler>>();
        var petIds = new List<int> { 1, 2 };
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Desc",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = petIds
        };
        var pets = new List<Pet>
        {
            new() { Id = petIds[0], Name = "Pet1" },
            new() { Id = petIds[1], Name = "Pet2" }
        };
        var createdEvent = new Event
        {
            Id = 10,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = new List<PetEvent>()
        };
        var expectedAssignedPets = new List<AssignedPetDto>
        {
            new() { Id = pets[0].Id, Name = pets[0].Name, PhotoUrl = pets[0].PhotoUrl },
            new() { Id = pets[1].Id, Name = pets[1].Name, PhotoUrl = pets[1].PhotoUrl } 
        };
        var handler = new CreateEventCommandHandler(petRepository, eventRepository, logger);
        
        petRepository.GetByIdsAsync(command.PetToAssignIds, Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.CreateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        eventRepository.AddPetsToEventAsync(Arg.Any<List<PetEvent>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(createdEvent.Id);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DateOfEvent.Should().Be(command.DateOfEvent);
        result.Reminder.Should().Be(command.Reminder);
        result.AssignedPets.Should().BeEquivalentTo(expectedAssignedPets);
        result.MissingPetIds.Should().BeNullOrEmpty();
    }

    [Test]
    public async Task ShouldCreateEventAssignExistingPetsAndReturnResponseWithMissingIdsWhenSomePetsDoNotExist()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<CreateEventCommandHandler>>();
        var petIds = new List<int> { 1, 2 };
        var missingPetIds = new List<int> { 3 };
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Desc",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = petIds.Concat(missingPetIds)
        };
        var pets = new List<Pet>
        {
            new() { Id = petIds[0], Name = "Pet1" },
            new() { Id = petIds[1], Name = "Pet2" }
        };
        var createdEvent = new Event
        {
            Id = 11,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = new List<PetEvent>()
        };
        var expectedAssignedPets = new List<AssignedPetDto>
        {
            new() { Id = pets[0].Id, Name = pets[0].Name, PhotoUrl = pets[0].PhotoUrl },
            new() { Id = pets[1].Id, Name = pets[1].Name, PhotoUrl = pets[1].PhotoUrl } 
        };
        var handler = new CreateEventCommandHandler(petRepository, eventRepository, logger);
        
        petRepository.GetByIdsAsync(command.PetToAssignIds, Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.CreateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        eventRepository.AddPetsToEventAsync(Arg.Any<List<PetEvent>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(createdEvent.Id);
        result.AssignedPets.Should().BeEquivalentTo(expectedAssignedPets);
        result.MissingPetIds.Should().BeEquivalentTo(missingPetIds);
    }

    [Test]
    public async Task ShouldCreateEventWithoutAssignPetsAndReturnResponseWithoutPetIdsWhenNoPetsProvided()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<CreateEventCommandHandler>>();
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Desc",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = new List<int>()
        };
        var pets = new List<Pet>();
        var createdEvent = new Event
        {
            Id = 12,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = new List<PetEvent>()
        };
        var handler = new CreateEventCommandHandler(petRepository, eventRepository, logger);
        
        petRepository.GetByIdsAsync(command.PetToAssignIds, Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.CreateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        eventRepository.AddPetsToEventAsync(Arg.Any<List<PetEvent>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(createdEvent.Id);
        result.AssignedPets.Should().BeEmpty();
        result.MissingPetIds.Should().BeEmpty();
    }

    [Test]
    public async Task ShouldCreateEventAndReturnAllMissingPetIdsWhenAllPetsFromCommandDoesNotExist()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<CreateEventCommandHandler>>();
        var petIds = new List<int> { 5, 6, 7 };
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Desc",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = petIds
        };
        var pets = new List<Pet>();
        var createdEvent = new Event
        {
            Id = 13,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = new List<PetEvent>()
        };
        var handler = new CreateEventCommandHandler(petRepository, eventRepository, logger);
        
        petRepository.GetByIdsAsync(command.PetToAssignIds, Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.CreateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        eventRepository.AddPetsToEventAsync(Arg.Any<List<PetEvent>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(createdEvent.Id);
        result.AssignedPets.Should().BeEmpty();
        result.MissingPetIds.Should().BeEquivalentTo(petIds);
    }

    [Test]
    public async Task ShouldCreateEventAndReturnResponseWithoutMissingIdsAndAssignedPetsWhenPetToAssignIdsIsNull()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<CreateEventCommandHandler>>();
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Desc",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            PetToAssignIds = null
        };
        var createdEvent = new Event
        {
            Id = 14,
            Title = command.Title,
            Description = command.Description,
            DateOfEvent = command.DateOfEvent,
            Reminder = command.Reminder,
            PetEvents = new List<PetEvent>()
        };
        var handler = new CreateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.CreateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>())
            .Returns(createdEvent);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(createdEvent.Id);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DateOfEvent.Should().Be(command.DateOfEvent);
        result.Reminder.Should().Be(command.Reminder);
        result.AssignedPets.Should().BeNull();
        result.MissingPetIds.Should().BeNull();
    }
}