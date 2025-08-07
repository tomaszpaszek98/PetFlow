using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Events.Commands.AddPetToEvent;

public class AddPetToEventCommandHandler : IRequestHandler<AddPetToEventCommand, AddPetToEventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IPetRepository _petRepository;

    public AddPetToEventCommandHandler(IEventRepository eventRepository, IPetRepository petRepository)
    {
        _eventRepository = eventRepository;
        _petRepository = petRepository;
    }

    public async Task<AddPetToEventResponse> Handle(AddPetToEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventEntity == null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        await ValidateIfPetExistsAsync(request.PetId, cancellationToken);
        ValidateIfPetIsNotAssignedToEvent(eventEntity, request.PetId);
        await AssignPetToEventAsync(request.EventId, request.PetId, cancellationToken);
        
        return CreateResponse(request.EventId, request.PetId);
    }
    
    private async Task ValidateIfPetExistsAsync(int petId, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdAsync(petId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), petId);
        }
    }
    
    private void ValidateIfPetIsNotAssignedToEvent(Event eventEntity, int petId)
    {
        if (eventEntity.PetEvents.Any(x => x.PetId == petId))
        {
            throw new ConflictingOperationException($"Pet with ID {petId} is already assigned to event with ID {eventEntity.Id}");
        }
    }
    
    private async Task AssignPetToEventAsync(int eventId, int petId, CancellationToken cancellationToken)
    {
        var petEvent = new PetEvent
        {
            EventId = eventId,
            PetId = petId
        };
        await _eventRepository.AddPetsToEventAsync(new List<PetEvent> { petEvent }, cancellationToken);
    }
    
    private AddPetToEventResponse CreateResponse(int eventId, int petId)
    {
        return new AddPetToEventResponse
        {
            EventId = eventId,
            PetId = petId,
            AssociatedAt = DateTime.UtcNow
        };
    }
}
