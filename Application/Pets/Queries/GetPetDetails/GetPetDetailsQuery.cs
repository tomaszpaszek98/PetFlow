using MediatR;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQuery : IRequest<PetDetailsResponse>
{
    public int PetId { get; set; }
}