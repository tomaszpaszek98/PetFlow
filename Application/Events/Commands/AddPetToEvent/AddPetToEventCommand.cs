using MediatR;

namespace Application.Events.Commands.AddPetToEvent;

public record AddPetToEventCommand : IRequest<AddPetToEventResponse>
{
    public int EventId { get; init; }
    public int PetId { get; init; }
}