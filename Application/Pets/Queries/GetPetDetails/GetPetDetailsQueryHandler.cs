using Application.Common.Interfaces.Repositories;
using Application.Pets.Common;
using Domain.Exceptions;
using MediatR;
using Persistance.Repositories;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandler : IRequestHandler<GetPetDetailsQuery, PetResponse>
{
    private readonly IPetRepository _repository;
    private readonly IEventRepository _eventRepository;
    
    public GetPetDetailsQueryHandler(IPetRepository repository, IEventRepository eventRepository)
    {
        _repository = repository;
        _eventRepository = eventRepository;
    }

    public async Task<PetResponse> Handle(GetPetDetailsQuery request, CancellationToken cancellationToken)
    {
        var pet = await _repository.GetByIdAsync(request.PetId, cancellationToken);
        if (pet == null)
        {
            throw new EntityNotFoundException($"Pet with id {request.PetId} is not exists.");
        }
        var petEvent = await _eventRepository.GetUpcomingEventForPetAsync(request.PetId, cancellationToken);
        
        return pet.MapToPetDetailsResponse(petEvent);
    }
}