using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Commands.AddPetToEvent;

public class AddPetToEventCommandHandler : IRequestHandler<AddPetToEventCommand, AddPetToEventResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILogger<AddPetToEventCommandHandler> _logger;

    public AddPetToEventCommandHandler(IEventRepository eventRepository,
        IPetRepository petRepository, ILogger<AddPetToEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<AddPetToEventResponse> Handle(AddPetToEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling AddPetToEventCommand for EventId: {EventId}, PetId: {PetId}", 
            request.EventId, request.PetId);
        var eventEntity = await _eventRepository.GetByIdWithPetEventsTrackedAsync(request.EventId, cancellationToken);
        if (eventEntity is null)
        {
            _logger.LogError("Event with ID {EventId} not found", request.EventId);
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        ValidateIfPetIsAssignedToEventOrThrow(eventEntity, request.PetId);
        
        var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet is null)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        await AddPetToEvent(eventEntity, pet, cancellationToken);
        
        _logger.LogInformation("Pet with ID {PetId} added successfully to Event with ID {EventId}", 
            request.PetId, request.EventId);
        return CreateResponse(request.EventId, request.PetId);
    }
    
    private async Task AddPetToEvent(Event eventEntity, Pet pet, CancellationToken cancellationToken)
    {
        var petEvent = new PetEvent 
        { 
            PetId = pet.Id,
            EventId = eventEntity.Id,
            Pet = pet,
            Event = eventEntity
        };
        eventEntity.PetEvents.Add(petEvent);
        await _eventRepository.UpdateAsync(eventEntity, cancellationToken);
    }
    
    private void ValidateIfPetIsAssignedToEventOrThrow(Event eventEntity, int petId)
    {
        if (eventEntity.PetEvents.Any(pe => pe.PetId == petId))
        {
            _logger.LogError($"Pet with ID {petId} is already assigned to event with ID {eventEntity.Id}");
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
