using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandler : IRequestHandler<GetPetDetailsQuery, PetDetailsResponse>
{
    private readonly IPetRepository _petRepository;
    
    public GetPetDetailsQueryHandler(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public async Task<PetDetailsResponse> Handle(GetPetDetailsQuery request, CancellationToken cancellationToken)
    {
        var pet = await _petRepository.GetByIdWithUpcomingEventAsync(request.PetId, cancellationToken);
        if (pet == null)
        {
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        return pet.MapToPetDetailsResponse();
    }
}