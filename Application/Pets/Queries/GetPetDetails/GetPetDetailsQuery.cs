using MediatR;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQuery : IRequest<PetDetailsVm>
{
    public int PetId { get; set; }
}