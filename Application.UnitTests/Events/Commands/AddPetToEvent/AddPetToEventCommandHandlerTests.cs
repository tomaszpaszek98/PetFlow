using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.AddPetToEvent;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Events.Commands.AddPetToEvent;

public class AddPetToEventCommandHandlerTests
{
    [Test]
    public async Task ShouldAddPetToEventAndReturnResponseWhenEventAndPetExistAndPetNotAssigned()
    {
        // GIVEN
        var eventId = 1;
        var petId = 2;
        var command = new AddPetToEventCommand
        {
            EventId = eventId,
            PetId = petId
        };
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Test Event",
            PetEvents = new List<PetEvent>()
        };
        var pet = new Pet { Id = petId, Name = "Test Pet" };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new AddPetToEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        petRepository.GetByIdAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.AddPetsToEventAsync(Arg.Any<IList<PetEvent>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.EventId.Should().Be(eventId);
        result.PetId.Should().Be(petId);
        result.AssociatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        
        Received.InOrder(() => {
            eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.GetByIdAsync(petId, Arg.Any<CancellationToken>());
            eventRepository.AddPetsToEventAsync(
                Arg.Is<IList<PetEvent>>(list => 
                    list.Count == 1 && 
                    list[0].EventId == eventId && 
                    list[0].PetId == petId), 
                Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var eventId = 1;
        var petId = 2;
        var command = new AddPetToEventCommand
        {
            EventId = eventId,
            PetId = petId
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new AddPetToEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns((Event)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(eventId.ToString()) && e.Message.Contains(nameof(Event)));
        
        await eventRepository.Received(1).GetByIdAsync(eventId, Arg.Any<CancellationToken>());
        await petRepository.DidNotReceive().GetByIdAsync(default);
        await eventRepository.DidNotReceive().AddPetsToEventAsync(default);
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var eventId = 1;
        var petId = 2;
        var command = new AddPetToEventCommand
        {
            EventId = eventId,
            PetId = petId
        };
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Test Event",
            PetEvents = new List<PetEvent>()
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new AddPetToEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        petRepository.GetByIdAsync(petId, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(petId.ToString()) && e.Message.Contains(nameof(Pet)));
        
        Received.InOrder(() =>
        {
            eventRepository.Received(1).GetByIdAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.Received(1).GetByIdAsync(petId, Arg.Any<CancellationToken>());
        });
        await eventRepository.DidNotReceive().AddPetsToEventAsync(default);
    }
    
    [Test]
    public async Task ShouldThrowConflictingOperationExceptionWhenPetAlreadyAssignedToEvent()
    {
        // GIVEN
        var eventId = 1;
        var petId = 2;
        var command = new AddPetToEventCommand
        {
            EventId = eventId,
            PetId = petId
        };
        var petEvent = new PetEvent { PetId = petId, EventId = eventId };
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Test Event",
            PetEvents = new List<PetEvent> { petEvent }
        };
        var pet = new Pet { Id = petId, Name = "Test Pet" };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new AddPetToEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        petRepository.GetByIdAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<ConflictingOperationException>()
            .Where(e => e.Message.Contains(petId.ToString()) && e.Message.Contains(eventId.ToString()));
        
        Received.InOrder(() =>
        {
            eventRepository.Received(1).GetByIdAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.Received(1).GetByIdAsync(petId, Arg.Any<CancellationToken>());
        });
        await eventRepository.DidNotReceive().AddPetsToEventAsync(default);
    }
}

