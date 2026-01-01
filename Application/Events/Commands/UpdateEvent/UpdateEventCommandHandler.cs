using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
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
        var existingEvent = await _eventRepository.GetByIdWithPetsAsync(request.Id, cancellationToken);
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
            eventEntity.Pets.Clear();
            _logger.LogInformation($"Removed all pet assignments for event {eventEntity.Id}.");
            return [];
        }

        var petsToAssignIds = request.AssignedPetsIds.Distinct().ToList();
        var petsToAssign = await _petRepository.GetByIdsAsync(petsToAssignIds, cancellationToken);
        
        ValidateAllPetsExistOrThrow(petsToAssign, petsToAssignIds);
        
        eventEntity.Pets.Clear();
        foreach (var pet in petsToAssign)
        {
            eventEntity.Pets.Add(pet);
        }

        return petsToAssign;
    }

    private static void ValidateAllPetsExistOrThrow(IList<Pet> foundPets, IList<int> requestedPetIds)
    {
        var foundPetIds = foundPets.Select(p => p.Id).Distinct();
        var missingPetIds = requestedPetIds.Where(id => !foundPetIds.Contains(id)).ToList();

        if (missingPetIds.Any())
        {
            throw new ValidationException($"Pets not found: {string.Join(", ", missingPetIds)}");
        }
    }
}

