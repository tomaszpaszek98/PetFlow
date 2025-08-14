using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandler : IRequestHandler<GetPetDetailsQuery, PetDetailsResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly IEventRepository _eventRepository;
    
    public GetPetDetailsQueryHandler(IPetRepository petRepository, IEventRepository eventRepository)
    {
        _petRepository = petRepository;
        _eventRepository = eventRepository;
    }

    public async Task<PetDetailsResponse> Handle(GetPetDetailsQuery request, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        var petEvents = await _eventRepository.GetEventsByPetIdAsync(request.PetId, cancellationToken);
        var upcomingEvent = petEvents
            .Where(e => e.DateOfEvent >= DateTime.UtcNow)
            .OrderBy(e => e.DateOfEvent)
            .FirstOrDefault();
        
        return pet.MapToPetDetailsResponse(upcomingEvent);
    }
}