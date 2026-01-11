using MediatR;

namespace Application.Pets.Queries.GetPetEvents;

public record GetPetEventsQuery : IRequest<PetEventsResponse>
{
    public int PetId { get; init; }
}