using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Pets.Queries.GetPetEvents;

public class GetPetEventsQueryHandler : IRequestHandler<GetPetEventsQuery, PetEventsResponse>
{
    private readonly IPetRepository _petRepository;
    private readonly ILogger<GetPetEventsQueryHandler> _logger;

    public GetPetEventsQueryHandler(IPetRepository petRepository, ILogger<GetPetEventsQueryHandler> logger)
    {
        _petRepository = petRepository;
        _logger = logger;
    }

    public async Task<PetEventsResponse> Handle(GetPetEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetPetEventsQuery for PetId: {PetId}", request.PetId);
        
        var pet = await _petRepository.GetByIdWithEventsAsync(request.PetId, cancellationToken);
        
        if (pet is null)
        {
            _logger.LogError("Pet with ID {PetId} not found", request.PetId);
            throw new NotFoundException(nameof(Pet), request.PetId);
        }
        
        _logger.LogInformation("Retrieved {Count} events for PetId: {PetId}", pet.Events.Count(), request.PetId);
        return pet.Events.MapToResponse();
    }
}