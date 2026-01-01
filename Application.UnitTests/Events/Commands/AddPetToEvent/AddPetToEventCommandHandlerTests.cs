using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.AddPetToEvent;
using Domain.Entities;
using Domain.Exceptions;

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
            Pets = new List<Pet>()
        };
        var pet = new Pet { Id = petId, Name = "Test Pet" };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new AddPetToEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        petRepository.GetByIdAsync(petId, Arg.Any<CancellationToken>())
            .Returns(pet);
        eventRepository.UpdateAsync(eventEntity, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);
        
        // THEN
        result.Should().NotBeNull();
        result.EventId.Should().Be(eventId);
        result.PetId.Should().Be(petId);
        result.AssociatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        
        Received.InOrder(() => {
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.GetByIdAsync(petId, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(eventEntity, Arg.Any<CancellationToken>());
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
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns((Event)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(eventId.ToString()) && e.Message.Contains(nameof(Event)));
        
        await eventRepository.Received(1).GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
        await petRepository.DidNotReceive().GetByIdAsync(default);
        await eventRepository.DidNotReceive().UpdateAsync(default);
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
            Pets = new List<Pet>()
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new AddPetToEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
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
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
            petRepository.GetByIdAsync(petId, Arg.Any<CancellationToken>());
        });
        await eventRepository.DidNotReceive().UpdateAsync(default);
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
        var pet = new Pet { Id = petId, Name = "Test Pet" };
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Test Event",
            Pets = new List<Pet> { pet }
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new AddPetToEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<ConflictingOperationException>()
            .Where(e => e.Message.Contains(petId.ToString()) && e.Message.Contains(eventId.ToString()));
        
        await eventRepository.Received(1).GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
        await petRepository.DidNotReceive().GetByIdAsync(default);
        await eventRepository.DidNotReceive().UpdateAsync(default);
    }
}
