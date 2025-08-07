using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.UpdateEvent;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Persistance.Repositories;

namespace Application.UnitTests.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandlerTests
{
    [Test]
    public async Task ShouldUpdateEventPropertiesAndAssignPetsWhenEventAndPetExists()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var petIds = new List<int> { 1, 2 };
        var command = new UpdateEventCommand
        {
            Id = 10,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            PetToAssignIds = petIds
        };
        var existingEvent = new Event
        {
            Id = 10,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            PetEvents = new List<PetEvent>()
        };
        var pets = new List<Pet>
        {
            new() { Id = petIds[0], Name = "Pet1", PhotoUrl = "url1" },
            new() { Id = petIds[1], Name = "Pet2", PhotoUrl = "url2" }
        };
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        petRepository.GetByIdsAsync(petIds, Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        eventRepository.AddPetsToEventAsync(
            Arg.Is<List<PetEvent>>(list => list.Count == 2 && list.All(pe => petIds.Contains(pe.PetId) && pe.EventId == command.Id)),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DateOfEvent.Should().Be(command.DateOfEvent);
        result.Reminder.Should().Be(command.Reminder);
        result.AssignedPets.Should().HaveCount(2);
        result.AssignedPets.Select(p => p.Id).Should().BeEquivalentTo(petIds);
        
        Received.InOrder(() => {
            eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(Arg.Is<Event>(e =>
                e.Id == command.Id &&
                e.Title == command.Title &&
                e.Description == command.Description &&
                e.DateOfEvent == command.DateOfEvent &&
                e.Reminder == command.Reminder),
                Arg.Any<CancellationToken>());
            petRepository.GetByIdsAsync(petIds, Arg.Any<CancellationToken>());
            eventRepository.AddPetsToEventAsync(
                Arg.Is<List<PetEvent>>(list =>
                    list.Count == 2 &&
                    list.All(pe => petIds.Contains(pe.PetId) && pe.EventId == command.Id)),
                Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var command = new UpdateEventCommand
        {
            Id = 999,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            PetToAssignIds = new List<int> { 1, 2 }
        };
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Event)null!);
        
        // WHEN & THEN
        await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Event with ID {command.Id} does not exist.");
            
        await eventRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await eventRepository.DidNotReceive().UpdateAsync(default, default);
        await petRepository.DidNotReceive().GetByIdsAsync(default, default);
        await eventRepository.DidNotReceive().AddPetsToEventAsync(default, default);
    }
    
    [Test]
    public async Task ShouldClearPetAssignmentsWhenPetToAssignIdsIsNull()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var command = new UpdateEventCommand
        {
            Id = 10,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            PetToAssignIds = null
        };
        var existingEvent = new Event
        {
            Id = 10,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            PetEvents = new List<PetEvent>
            {
                new() { PetId = 1, EventId = 10 },
                new() { PetId = 2, EventId = 10 }
            }
        };
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        eventRepository.AddPetsToEventAsync(Arg.Is<List<PetEvent>>(list => list.Count == 0), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.AssignedPets.Should().BeEmpty();
        
        Received.InOrder(() => {
            eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>());
            eventRepository.AddPetsToEventAsync(Arg.Is<List<PetEvent>>(list => list.Count == 0), Arg.Any<CancellationToken>());
        });
        await petRepository.DidNotReceive().GetByIdsAsync(default, default);
    }
    
    [Test]
    public async Task ShouldClearPetAssignmentsWhenPetToAssignIdsIsEmpty()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var command = new UpdateEventCommand
        {
            Id = 10,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            PetToAssignIds = new List<int>()
        };
        var existingEvent = new Event
        {
            Id = 10,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            PetEvents = new List<PetEvent>
            {
                new() { PetId = 1, EventId = 10 },
                new() { PetId = 2, EventId = 10 }
            }
        };
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        eventRepository.AddPetsToEventAsync(Arg.Is<List<PetEvent>>(list => list.Count == 0), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.AssignedPets.Should().BeEmpty();
        
        Received.InOrder(() => {
            eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>());
            eventRepository.AddPetsToEventAsync(Arg.Is<List<PetEvent>>(list => list.Count == 0), Arg.Any<CancellationToken>());
        });
        await petRepository.DidNotReceive().GetByIdsAsync(default, default);
    }
    
    [Test]
    public async Task ShouldSkipPetAssignmentWhenNoPetsFound()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var petIds = new List<int> { 999, 998 };
        var command = new UpdateEventCommand
        {
            Id = 10,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            PetToAssignIds = petIds
        };
        var existingEvent = new Event
        {
            Id = 10,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            PetEvents = new List<PetEvent>()
        };
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        petRepository.GetByIdsAsync(petIds, Arg.Any<CancellationToken>())
            .Returns(new List<Pet>());
        eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.AssignedPets.Should().BeEmpty();
        
        Received.InOrder(() => {
            eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>());
            petRepository.GetByIdsAsync(petIds, Arg.Any<CancellationToken>());
        });
        await eventRepository.DidNotReceive().AddPetsToEventAsync(default, default);
    }
    
    [Test]
    public async Task ShouldAssignOnlyFoundPetsWhenListOfPetsToAssignContainsExistingAndNotExistingPets()
    {
        // GIVEN
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var requestedPetIds = new List<int> { 1, 2, 999 }; // 999 nie istnieje
        var existingPetIds = new List<int> { 1, 2 };
        var command = new UpdateEventCommand
        {
            Id = 10,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            PetToAssignIds = requestedPetIds
        };
        var existingEvent = new Event
        {
            Id = 10,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            PetEvents = new List<PetEvent>()
        };
        var foundPets = new List<Pet>
        {
            new() { Id = existingPetIds[0], Name = "Pet1", PhotoUrl = "url1" },
            new() { Id = existingPetIds[1], Name = "Pet2", PhotoUrl = "url2" }
        };
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        petRepository.GetByIdsAsync(requestedPetIds, Arg.Any<CancellationToken>())
            .Returns(foundPets);
        eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        eventRepository.AddPetsToEventAsync(
            Arg.Is<List<PetEvent>>(list =>
                list.Count == 2 &&
                list.All(pe => existingPetIds.Contains(pe.PetId) && pe.EventId == command.Id)),
            Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.AssignedPets.Should().HaveCount(2);
        result.AssignedPets.Select(p => p.Id).Should().BeEquivalentTo(existingPetIds);
        result.AssignedPets.Select(p => p.Id).Should().NotContain(999);
        
        Received.InOrder(() => {
            eventRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(Arg.Is<Event>(e => e.Id == command.Id), Arg.Any<CancellationToken>());
            petRepository.GetByIdsAsync(requestedPetIds, Arg.Any<CancellationToken>());
            eventRepository.AddPetsToEventAsync(
                Arg.Is<List<PetEvent>>(list => 
                    list.Count == 2 && 
                    list.All(pe => existingPetIds.Contains(pe.PetId) && pe.EventId == command.Id)),
                Arg.Any<CancellationToken>());
        });
        logger.DidNotReceive().LogWarning(default);
    }
}
