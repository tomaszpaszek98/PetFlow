using Application.Pets.Common;
using MediatR;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQuery : IRequest<PetResponse>
{
    public int PetId { get; set; }
}