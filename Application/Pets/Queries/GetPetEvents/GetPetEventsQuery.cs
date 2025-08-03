using Domain.Entities;
using MediatR;

namespace Application.Pets.Queries.GetPetEvents;

public class GetPetEventsQuery : IRequest<PetEventsResponse>
{
    public int PetId { get; set; }
}