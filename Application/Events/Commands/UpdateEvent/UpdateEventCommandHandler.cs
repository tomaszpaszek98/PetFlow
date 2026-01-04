using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, UpdateEventResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<UpdateEventCommandHandler> _logger;

    public UpdateEventCommandHandler(IPetRepository petRepository,
        IEventRepository eventRepository,
        ILogger<UpdateEventCommandHandler> logger)
    {
        _petRepository = petRepository;
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<UpdateEventResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _eventRepository.GetByIdWithPetEventsTrackedAsync(request.Id, cancellationToken);
        if (existingEvent is null)
        {
            throw new NotFoundException(nameof(Event), request.Id);
        }

        var assignedPets = await HandlePetAssignments(request, existingEvent, cancellationToken);
        
        UpdateEventProperties(existingEvent, request);
        await _eventRepository.UpdateAsync(existingEvent, cancellationToken);
        
        return existingEvent.MapToUpdateResponse(assignedPets);
    }
    
    private static void UpdateEventProperties(Event eventEntity, UpdateEventCommand request)
    {
        eventEntity.Title = request.Title;
        eventEntity.Description = request.Description;
        eventEntity.DateOfEvent = request.DateOfEvent;
        eventEntity.Reminder = request.Reminder;
    }
    
    private async Task<IList<Pet>> HandlePetAssignments(UpdateEventCommand request, Event eventEntity, CancellationToken cancellationToken)
    {
        if (!request.AssignedPetsIds.Any())
        {
            eventEntity.PetEvents.Clear();
            _logger.LogInformation($"Removed all pet assignments for event {eventEntity.Id}.");
            return [];
        }

        var petsToAssignIds = request.AssignedPetsIds.Distinct().ToList();
        var petsToAssign = await _petRepository.GetByIdsAsync(petsToAssignIds, cancellationToken);
        
        ValidateAllPetsExistOrThrow(petsToAssign, petsToAssignIds);
        SynchronizePetEvents(eventEntity, petsToAssign, petsToAssignIds);

        return petsToAssign;
    }

    private static void SynchronizePetEvents(Event eventEntity, IList<Pet> petsToAssign, IList<int> petsToAssignIds)
    {
        var currentPetEventIds = eventEntity.PetEvents.Select(pe => pe.PetId).ToList();
        var petEventsToRemove = eventEntity.PetEvents.Where(pe => !petsToAssignIds.Contains(pe.PetId)).ToList();
        var petsToAdd = petsToAssign.Where(p => !currentPetEventIds.Contains(p.Id)).ToList();
        
        foreach (var petEvent in petEventsToRemove)
        {
            eventEntity.PetEvents.Remove(petEvent);
        }
        
        foreach (var pet in petsToAdd)
        {
            eventEntity.PetEvents.Add(new PetEvent 
            { 
                PetId = pet.Id,
                EventId = eventEntity.Id,
                Pet = pet
            });
        }
    }

    private static void ValidateAllPetsExistOrThrow(IList<Pet> foundPets, IList<int> requestedPetIds)
    {
        var foundPetIds = foundPets.Select(p => p.Id).Distinct();
        var missingPetIds = requestedPetIds.Where(id => !foundPetIds.Contains(id)).ToList();

        if (missingPetIds.Any())
        {
            throw new NotFoundException($"Pets not found. Ids: {string.Join(", ", missingPetIds)}");
        }
    }
}

