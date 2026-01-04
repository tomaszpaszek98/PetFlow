using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Pets.Queries.GetPetEvents;

public class GetPetEventsQueryHandler : IRequestHandler<GetPetEventsQuery, PetEventsResponse>
{
    private readonly IPetRepository _petRepository;

    public GetPetEventsQueryHandler(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public async Task<PetEventsResponse> Handle(GetPetEventsQuery request, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdWithEventsAsync(request.PetId, cancellationToken);
        
        if (pet is null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        return pet.Events.MapToResponse();
    }
}