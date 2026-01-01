using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.UpdateEvent;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandlerTests
{
    [Test]
    public async Task ShouldUpdateEventPropertiesAndAssignPetsWhenEventAndPetsExist()
    {
        // GIVEN
        var petIds = new List<int> { 1, 2 };
        var eventId = 10;
        var command = new UpdateEventCommand
        {
            Id = eventId,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            AssignedPetsIds = petIds
        };
        var existingEvent = new Event
        {
            Id = eventId,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            Pets = new List<Pet>()
        };
        var pets = new List<Pet>
        {
            new() { Id = 1, Name = "Pet1", PhotoUrl = "url1" },
            new() { Id = 2, Name = "Pet2", PhotoUrl = "url2" }
        };
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(petIds)), Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.UpdateAsync(existingEvent, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DateOfEvent.Should().Be(command.DateOfEvent);
        result.Reminder.Should().Be(command.Reminder);
        result.AssignedPets.Should().HaveCount(2);
        result.AssignedPets.Select(p => p.Id).Should().BeEquivalentTo(petIds);
        
        Received.InOrder(() => {
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(petIds)), Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(existingEvent, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var eventId = 999;
        var command = new UpdateEventCommand
        {
            Id = eventId,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            AssignedPetsIds = new List<int> { 1, 2 }
        };
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns((Event)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Event)) && e.Message.Contains(eventId.ToString()));
        await eventRepository.Received(1).GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
        await petRepository.DidNotReceive().GetByIdsAsync(default);
        await eventRepository.DidNotReceive().UpdateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldClearPetAssignmentsWhenAssignedPetsIdsIsEmpty()
    {
        // GIVEN
        var eventId = 10;
        var emptyPetIds = new List<int>();
        var command = new UpdateEventCommand
        {
            Id = eventId,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            AssignedPetsIds = emptyPetIds
        };
        var existingEvent = new Event
        {
            Id = eventId,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            Pets = new List<Pet>
            {
                new() { Id = 1, Name = "Pet1", PhotoUrl = "url1" },
                new() { Id = 2, Name = "Pet2", PhotoUrl = "url2" }
            }
        };
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        eventRepository.UpdateAsync(existingEvent, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.AssignedPets.Should().BeEmpty();
        
        Received.InOrder(() => {
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(existingEvent, Arg.Any<CancellationToken>());
        });
        await petRepository.DidNotReceive().GetByIdsAsync(default);
    }
    
    [Test]
    public async Task ShouldSkipPetAssignmentWhenNoPetsFound()
    {
        // GIVEN
        var requestedPetIds = new List<int> { 999, 998 };
        var eventId = 10;
        var command = new UpdateEventCommand
        {
            Id = eventId,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            AssignedPetsIds = requestedPetIds
        };
        var existingEvent = new Event
        {
            Id = eventId,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            Pets = new List<Pet>()
        };
        var foundPets = new List<Pet>();
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>())
            .Returns(foundPets);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<ValidationException>();
        
        Received.InOrder(() => {
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>());
        });
        await eventRepository.DidNotReceive().UpdateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowValidationExceptionWhenSomeRequestedPetsDoNotExist()
    {
        // GIVEN
        var requestedPetIds = new List<int> { 1, 2, 999 };
        var eventId = 10;
        var command = new UpdateEventCommand
        {
            Id = eventId,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            AssignedPetsIds = requestedPetIds
        };
        var existingEvent = new Event
        {
            Id = eventId,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            Pets = new List<Pet>()
        };
        var foundPets = new List<Pet>
        {
            new() { Id = 1, Name = "Pet1", PhotoUrl = "url1" },
            new() { Id = 2, Name = "Pet2", PhotoUrl = "url2" }
        };
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>())
            .Returns(foundPets);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);
        
        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Message.Contains("999"));
        
        Received.InOrder(() => {
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(requestedPetIds)), Arg.Any<CancellationToken>());
        });
        await eventRepository.DidNotReceive().UpdateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldRemoveDuplicatePetIdsBeforeAssigningWhenAssignedPetsIdsContainsDuplicates()
    {
        // GIVEN
        var duplicatePetIds = new List<int> { 1, 1, 2, 2, 3 };
        var uniquePetIds = new List<int> { 1, 2, 3 };
        var eventId = 10;
        var command = new UpdateEventCommand
        {
            Id = eventId,
            Title = "Updated Event",
            Description = "Updated Description",
            DateOfEvent = DateTime.Today.AddDays(2),
            Reminder = false,
            AssignedPetsIds = duplicatePetIds
        };
        var existingEvent = new Event
        {
            Id = eventId,
            Title = "Original Event",
            Description = "Original Description",
            DateOfEvent = DateTime.Today.AddDays(1),
            Reminder = true,
            Created = DateTime.Today.AddDays(-1),
            Modified = null,
            Pets = new List<Pet>()
        };
        var pets = new List<Pet>
        {
            new() { Id = 1, Name = "Pet1", PhotoUrl = "url1" },
            new() { Id = 2, Name = "Pet2", PhotoUrl = "url2" },
            new() { Id = 3, Name = "Pet3", PhotoUrl = "url3" }
        };
        var petRepository = Substitute.For<IPetRepository>();
        var eventRepository = Substitute.For<IEventRepository>();
        var logger = Substitute.For<ILogger<UpdateEventCommandHandler>>();
        var handler = new UpdateEventCommandHandler(petRepository, eventRepository, logger);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(existingEvent);
        petRepository.GetByIdsAsync(Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(uniquePetIds)), Arg.Any<CancellationToken>())
            .Returns(pets);
        eventRepository.UpdateAsync(existingEvent, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(eventId);
        result.AssignedPets.Should().HaveCount(3);

        await petRepository.Received(1).GetByIdsAsync(
            Arg.Is<IEnumerable<int>>(ids => ids.SequenceEqual(uniquePetIds)),
            Arg.Any<CancellationToken>());
    }
}

