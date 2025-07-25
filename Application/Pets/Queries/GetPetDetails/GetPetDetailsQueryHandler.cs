using Application.Common.Interfaces.Repositories;
using Application.Pets.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandler : IRequestHandler<GetPetDetailsQuery, PetDetailsResponse>
{
    private readonly IPetRepository _repository;
    private readonly IEventRepository _eventRepository;
    
    public GetPetDetailsQueryHandler(IPetRepository repository, IEventRepository eventRepository)
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<PetDetailsResponse> Handle(GetPetDetailsQuery request, CancellationToken cancellationToken)
    {
        var pet = await _repository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        var petEvent = await _eventRepository.GetUpcomingEventForPetAsync(request.PetId, cancellationToken);
        
        return pet.MapToPetDetailsResponse(petEvent);
    }
}