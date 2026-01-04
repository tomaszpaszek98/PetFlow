using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandHandler(IPetRepository petRepository,
        IEventRepository eventRepository)
    {
        _petRepository = petRepository;
        _eventRepository = eventRepository;
    }

    public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var requestedPetsToAssignIds = request.PetToAssignIds?.Distinct().ToList() ?? [];
        var petsToAssign = await _petRepository.GetByIdsAsync(requestedPetsToAssignIds, cancellationToken);

        ValidateAllPetsExistOrThrow(petsToAssign, requestedPetsToAssignIds);

        var requestedEvent = request.MapToEvent(petsToAssign);
        var createdEvent = await _eventRepository.CreateAsync(requestedEvent, cancellationToken);

        return createdEvent.MapToResponse(petsToAssign);
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

