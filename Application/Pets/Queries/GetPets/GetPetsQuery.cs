using Application.Pets.Common;
using MediatR;

namespace Application.Pets.Queries.GetPets;

public class GetPetsQuery : IRequest<PetsResponse>
{
}

