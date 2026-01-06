using MediatR;

namespace Application.Pets.Queries.GetPetDetails;

public record GetPetDetailsQuery : IRequest<PetDetailsResponse>
{
    public int PetId { get; init; }
}