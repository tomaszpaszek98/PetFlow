using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Pets.Queries.GetPets;

public class GetPetsQueryHandler : IRequestHandler<GetPetsQuery, PetsResponse>
{
    private readonly IPetRepository _repository;

    public GetPetsQueryHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PetsResponse> Handle(GetPetsQuery request, CancellationToken cancellationToken)
    {
        var pets = await _repository.GetAllAsync(cancellationToken);

        return pets.MapToResponse();
    }
}
