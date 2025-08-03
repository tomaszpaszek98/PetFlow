using Domain.Entities;
using MediatR;

namespace Application.Pets.Queries.GetPetEvents;

public class GetPetEventsQuery : IRequest<IEnumerable<Event>>
{
    public int PetId { get; set; }
}