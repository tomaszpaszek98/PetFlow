using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryHandler : IRequestHandler<GetEventDetailsQuery, EventDetailsResponse>
{
    private readonly IEventRepository _eventRepository;
    private readonly IPetRepository _petRepository;

    public GetEventDetailsQueryHandler(IEventRepository eventRepository, IPetRepository petRepository)
    {
        _eventRepository = eventRepository;
        _petRepository = petRepository;
    }

    public async Task<EventDetailsResponse> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
    {
        var eventDetails = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);

        if (eventDetails is null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }

        var assignedPetIds = eventDetails.PetEvents.Select(x => x.PetId).ToList();
        var assignedPets = new List<Pet>();
        
        if (assignedPetIds.Any())
        {
            assignedPets = (await _petRepository.GetByIdsAsync(assignedPetIds, cancellationToken)).ToList();
        }

        return eventDetails.MapToResponse(assignedPets);
    }
}