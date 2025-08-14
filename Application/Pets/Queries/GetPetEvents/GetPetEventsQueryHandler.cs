using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Pets.Queries.GetPetEvents;

public class GetPetEventsQueryHandler : IRequestHandler<GetPetEventsQuery, PetEventsResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly IEventRepository _eventRepository;

    public GetPetEventsQueryHandler(IPetRepository petRepository, IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
        _petRepository = petRepository;
    }

    public async Task<PetEventsResponse> Handle(GetPetEventsQuery request, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdAsync(request.PetId);
        if (pet is null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var events = await _eventRepository.GetEventsByPetIdAsync(request.PetId, cancellationToken);

        return events.MapToResponse();
    }
}

