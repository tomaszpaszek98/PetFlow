using MediatR;

namespace Application.Pets.Queries.GetPets;

public record GetPetsQuery : IRequest<PetsResponse>;