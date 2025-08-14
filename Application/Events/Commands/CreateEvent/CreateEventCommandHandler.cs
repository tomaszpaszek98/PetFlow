using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistance.Repositories;

namespace Application.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<CreateEventCommandHandler> _logger;

    public CreateEventCommandHandler(IPetRepository petRepository,
        IEventRepository eventRepository, 
        ILogger<CreateEventCommandHandler> logger)
    {
        _petRepository = petRepository;
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var requestedEvent = request.MapToEvent();
        var createdEvent = await _eventRepository.CreateAsync(requestedEvent, cancellationToken);

        if (request.PetToAssignIds is null || !request.PetToAssignIds.Any())
        {
            return createdEvent.MapToResponse();
        }

        var petsToAssign = await GetPetsToAssign(request.PetToAssignIds, cancellationToken);
        var petIdsToAssign = petsToAssign.Select(p => p.Id).ToHashSet();
       
        if (petIdsToAssign.Any())
        {
            await AssignPetsToEvent(petIdsToAssign, createdEvent.Id, cancellationToken);
        }
    
        var missingPetIds = GetIdsOfMissingPets(request.PetToAssignIds, petIdsToAssign);    
        return createdEvent.MapToResponse(petsToAssign, missingPetIds);
    }

    private async Task<List<Pet>> GetPetsToAssign(IEnumerable<int> petIds, CancellationToken cancellationToken)
    {
        return (await _petRepository.GetByIdsAsync(petIds, cancellationToken)).ToList();
    }
    
    private List<int> GetIdsOfMissingPets(IEnumerable<int> petIds, HashSet<int> foundIds)
    {   
        var missingPetIds = petIds.Where(id => !foundIds.Contains(id)).ToList();
        _logger.LogWarning($"Found {missingPetIds.Count} missing pet ids. Missing IDs: {string.Join(", ", missingPetIds)}");
        
        return missingPetIds;
    }
    
    private async Task AssignPetsToEvent(HashSet<int> petsToAssign, int createdEventId, CancellationToken cancellationToken)
    {
        var petEvents = petsToAssign
            .Select(petId => new PetEvent { PetId = petId, EventId = createdEventId })
            .ToList();
        
        await _eventRepository.AddPetsToEventAsync(petEvents, cancellationToken);
    }
}