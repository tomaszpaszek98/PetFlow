using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandler : IRequestHandler<GetPetDetailsQuery, PetDetailsResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetPetDetailsQueryHandler> _logger;
    
    public GetPetDetailsQueryHandler(IPetRepository petRepository, ILogger<GetPetDetailsQueryHandler> logger)
    {
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<PetDetailsResponse> Handle(GetPetDetailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetPetDetailsQuery for PetId: {PetId}", request.PetId);
        
        var pet = await _petRepository.GetByIdWithUpcomingEventAsync(request.PetId, cancellationToken);
        if (pet == null)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        _logger.LogInformation("Pet details retrieved successfully for PetId: {PetId}", request.PetId);
        return pet.MapToPetDetailsResponse();
    }
}