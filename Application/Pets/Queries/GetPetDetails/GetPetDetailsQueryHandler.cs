using MediatR;

namespace Application.Pets.Queries.GetPetDetails;

public class GetPetDetailsQueryHandler : IRequestHandler<GetPetDetailsQuery, PetDetailsVm>
{
    public Task<PetDetailsVm> Handle(GetPetDetailsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}