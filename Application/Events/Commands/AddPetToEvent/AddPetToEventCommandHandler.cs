using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

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
        var eventEntity = await _eventRepository.GetByIdWithPetsAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        ValidateIfPetIsAssignedToEventOrThrow(eventEntity, request.PetId);
        
        var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet is null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        await AddPetToEvent(eventEntity, pet, cancellationToken);
        
        return CreateResponse(request.EventId, request.PetId);
    }
    
    private async Task AddPetToEvent(Event eventEntity, Pet pet, CancellationToken cancellationToken)
    {
        eventEntity.Pets.Add(pet);
        await _eventRepository.UpdateAsync(eventEntity, cancellationToken);
    }
    
    private void ValidateIfPetIsAssignedToEventOrThrow(Event eventEntity, int petId)
    {
        if (eventEntity.Pets.Any(x => x.Id == petId))
        {
            throw new ConflictingOperationException($"Pet with ID {petId} is already assigned to event with ID {eventEntity.Id}");
        }
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
