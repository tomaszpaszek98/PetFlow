using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistance.Repositories;

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
        var existingEvent = await _eventRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (existingEvent is null)
        {
            throw new NotFoundException(nameof(Event), request.Id);
        }
        
        UpdateEventProperties(existingEvent, request);
        await _eventRepository.UpdateAsync(existingEvent, cancellationToken);
        
        var assignedPets = await HandlePetAssignments(request, existingEvent.Id, cancellationToken);
        
        return existingEvent.MapToUpdateResponse(assignedPets);
    }
    
    private static void UpdateEventProperties(Event eventEntity, UpdateEventCommand request)
    {
        eventEntity.Title = request.Title;
        eventEntity.Description = request.Description;
        eventEntity.DateOfEvent = request.DateOfEvent;
        eventEntity.Reminder = request.Reminder;
    }
    
    private async Task<List<Pet>> HandlePetAssignments(UpdateEventCommand request, int eventId, CancellationToken cancellationToken)
    {
        var petsToAssign = new List<Pet>();
        
        if (request.PetToAssignIds != null && request.PetToAssignIds.Any())
        {
            petsToAssign = await GetPetsToAssign(request.PetToAssignIds, cancellationToken);
            await AssignPetsToEvent(petsToAssign, eventId, cancellationToken);
        }
        else
        {
            await ClearPetAssignments(eventId, cancellationToken);
        }
        
        return petsToAssign;
    }
    
    private async Task<List<Pet>> GetPetsToAssign(IEnumerable<int> petIds, CancellationToken cancellationToken)
    {
        var pets = (await _petRepository.GetByIdsAsync(petIds, cancellationToken)).ToList();
        
        if (!pets.Any())
        {
            _logger.LogWarning($"None of the provided pet ids ({string.Join(", ", petIds)}) were found.");
        }
        
        return pets;
    }
    
    private async Task AssignPetsToEvent(List<Pet> petsToAssign, int eventId, CancellationToken cancellationToken)
    {
        if (!petsToAssign.Any())
            return;
            
        var petIdsToAssign = petsToAssign.Select(p => p.Id).ToHashSet();
        var petEvents = petIdsToAssign
            .Select(petId => new PetEvent { PetId = petId, EventId = eventId })
            .ToList();
        
        await _eventRepository.AddPetsToEventAsync(petEvents, cancellationToken);
        _logger.LogInformation($"Updated pet assignments for event {eventId}. Assigned {petIdsToAssign.Count} pets.");
    }
    
    private async Task ClearPetAssignments(int eventId, CancellationToken cancellationToken)
    {
        await _eventRepository.AddPetsToEventAsync(new List<PetEvent>(), cancellationToken);
        _logger.LogInformation($"Removed all pet assignments for event {eventId}.");
    }
}
