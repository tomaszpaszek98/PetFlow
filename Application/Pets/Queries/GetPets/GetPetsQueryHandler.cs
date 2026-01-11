using Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Pets.Queries.GetPets;

public class GetPetsQueryHandler : IRequestHandler<GetPetsQuery, PetsResponse>
{
    private readonly IPetRepository _repository;
    private readonly ILogger<GetPetsQueryHandler> _logger;

    public GetPetsQueryHandler(IPetRepository repository, ILogger<GetPetsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PetsResponse> Handle(GetPetsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetPetsQuery");
        
        var pets = (await _repository.GetAllAsync(cancellationToken)).ToList();

        _logger.LogInformation("Retrieved {Count} pets successfully", pets.Count);
        return pets.MapToResponse();
    }
}
