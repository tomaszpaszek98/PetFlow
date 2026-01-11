using MediatR;

namespace Application.Events.Commands.DeletePetFromEvent;

public record DeletePetFromEventCommand : IRequest
{
    public int EventId { get; init; }
    public int PetId { get; init; }
}
