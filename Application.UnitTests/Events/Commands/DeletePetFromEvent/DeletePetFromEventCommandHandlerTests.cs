using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.DeletePetFromEvent;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Events.Commands.DeletePetFromEvent;

public class DeletePetFromEventCommandHandlerTests
{
    [Test]
    public async Task ShouldCompleteSuccessfullyWhenPetIsRemovedFromEvent()
    {
        // GIVEN
        var command = new DeletePetFromEventCommand { EventId = 1, PetId = 2 };
        var eventEntity = new Event 
        { 
            Id = command.EventId, 
            Title = "Test Event",
            PetEvents = new List<PetEvent>() 
        };
        var pet = new Pet { Id = command.PetId, Name = "Test Pet" };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeletePetFromEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(command.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>()).Returns(pet);
        eventRepository.RemovePetFromEventAsync(command.EventId, command.PetId, Arg.Any<CancellationToken>()).Returns(true);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().NotThrowAsync();
        Received.InOrder(() =>
        {
            eventRepository.Received(1).GetByIdAsync(command.EventId, Arg.Any<CancellationToken>());
            petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
            eventRepository.Received(1).RemovePetFromEventAsync(command.EventId, command.PetId, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var command = new DeletePetFromEventCommand { EventId = 99, PetId = 2 };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeletePetFromEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(command.EventId, Arg.Any<CancellationToken>()).Returns((Event)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Event)) && e.Message.Contains(command.EventId.ToString()));
        await eventRepository.Received(1).GetByIdAsync(command.EventId, Arg.Any<CancellationToken>());
        await petRepository.DidNotReceive().GetByIdAsync(default);
        await eventRepository.DidNotReceive().RemovePetFromEventAsync(default, default);
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new DeletePetFromEventCommand { EventId = 1, PetId = 99 };
        var eventEntity = new Event 
        { 
            Id = command.EventId, 
            Title = "Test Event",
            PetEvents = new List<PetEvent>() 
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeletePetFromEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(command.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>()).Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Pet)) && e.Message.Contains(command.PetId.ToString()));
        await eventRepository.Received(1).GetByIdAsync(command.EventId, Arg.Any<CancellationToken>());
        await petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
        await eventRepository.DidNotReceive().RemovePetFromEventAsync(default, default);
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetIsNotAssignedToEvent()
    {
        // GIVEN
        var command = new DeletePetFromEventCommand { EventId = 1, PetId = 2 };
        var eventEntity = new Event 
        { 
            Id = command.EventId, 
            Title = "Test Event",
            PetEvents = new List<PetEvent>() 
        };
        var pet = new Pet { Id = command.PetId, Name = "Test Pet" };
        var eventRepository = Substitute.For<IEventRepository>();
        var petRepository = Substitute.For<IPetRepository>();
        var handler = new DeletePetFromEventCommandHandler(eventRepository, petRepository);
        
        eventRepository.GetByIdAsync(command.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);
        petRepository.GetByIdAsync(command.PetId, Arg.Any<CancellationToken>()).Returns(pet);
        eventRepository.RemovePetFromEventAsync(command.EventId, command.PetId, Arg.Any<CancellationToken>()).Returns(false);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(command.PetId.ToString()) && e.Message.Contains(command.EventId.ToString()));
        Received.InOrder(() =>
        {
            eventRepository.Received(1).GetByIdAsync(command.EventId, Arg.Any<CancellationToken>());
            petRepository.Received(1).GetByIdAsync(command.PetId, Arg.Any<CancellationToken>());
            eventRepository.Received(1).RemovePetFromEventAsync(command.EventId, command.PetId, Arg.Any<CancellationToken>());
        });
    }
}
