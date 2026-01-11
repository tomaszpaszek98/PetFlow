using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

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
        _logger.LogInformation("Handling CreateEventCommand with Title: {Title}", request.Title);
        _logger.LogDebug(
            "CreateEventCommand details - Title: {Title}, Description: {Description}, DateOfEvent: {DateOfEvent}, Reminder: {Reminder}, PetIds: {PetIds}",
            request.Title,
            request.Description,
            request.DateOfEvent,
            request.Reminder,
            request.PetToAssignIds != null ? string.Join(", ", request.PetToAssignIds) : "none");
        
        var requestedPetsToAssignIds = request.PetToAssignIds?.Distinct().ToList() ?? [];
        var petsToAssign = await _petRepository.GetByIdsAsync(requestedPetsToAssignIds, cancellationToken);

        ValidateAllPetsExistOrThrow(petsToAssign, requestedPetsToAssignIds);

        var requestedEvent = request.MapToEvent(petsToAssign);
        var createdEvent = await _eventRepository.CreateAsync(requestedEvent, cancellationToken);

        _logger.LogInformation("Event created successfully with ID {EventId}", createdEvent.Id);
        return createdEvent.MapToResponse(petsToAssign);
    }

    private void ValidateAllPetsExistOrThrow(IList<Pet> foundPets, IList<int> requestedPetIds)
    {
        var foundPetIds = foundPets.Select(p => p.Id).Distinct();
        var missingPetIds = requestedPetIds.Where(id => !foundPetIds.Contains(id)).ToList();

        if (missingPetIds.Any())
        {
            _logger.LogError("Pets not found during event creation. Pet Ids: {PetIds}", string.Join(", ", missingPetIds));
            throw new NotFoundException($"Pets not found. Ids: {string.Join(", ", missingPetIds)}");
        }
    }
}
