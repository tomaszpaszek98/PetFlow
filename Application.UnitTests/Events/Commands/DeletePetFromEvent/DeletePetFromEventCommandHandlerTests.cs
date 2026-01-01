using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.DeletePetFromEvent;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.Events.Commands.DeletePetFromEvent;

public class DeletePetFromEventCommandHandlerTests
{
    [Test]
    public async Task ShouldCompleteSuccessfullyWhenPetIsRemovedFromEvent()
    {
        // GIVEN
        var eventId = 1;
        var petId = 2;
        var command = new DeletePetFromEventCommand { EventId = eventId, PetId = petId };
        var pet = new Pet { Id = petId, Name = "Test Pet" };
        var eventEntity = new Event 
        { 
            Id = eventId, 
            Title = "Test Event",
            Pets = new List<Pet> { pet }
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new DeletePetFromEventCommandHandler(eventRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        eventRepository.UpdateAsync(eventEntity, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().NotThrowAsync();
        Received.InOrder(() =>
        {
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
            eventRepository.UpdateAsync(eventEntity, Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var eventId = 99;
        var petId = 2;
        var command = new DeletePetFromEventCommand { EventId = eventId, PetId = petId };
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new DeletePetFromEventCommandHandler(eventRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns((Event)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Event)) && e.Message.Contains(eventId.ToString()));
        await eventRepository.Received(1).GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
        await eventRepository.DidNotReceive().UpdateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }
    
    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetIsNotAssignedToEvent()
    {
        // GIVEN
        var eventId = 1;
        var petId = 99;
        var command = new DeletePetFromEventCommand { EventId = eventId, PetId = petId };
        var eventEntity = new Event 
        { 
            Id = eventId, 
            Title = "Test Event",
            Pets = new List<Pet>()
        };
        var eventRepository = Substitute.For<IEventRepository>();
        var handler = new DeletePetFromEventCommandHandler(eventRepository);
        
        eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(eventEntity);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(petId.ToString()) && e.Message.Contains(eventId.ToString()));
        Received.InOrder(() =>
        {
            eventRepository.GetByIdWithPetsAsync(eventId, Arg.Any<CancellationToken>());
        });
        await eventRepository.DidNotReceive().UpdateAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }
}
