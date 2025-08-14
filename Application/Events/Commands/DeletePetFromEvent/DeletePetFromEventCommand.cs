using MediatR;

namespace Application.Events.Commands.DeletePetFromEvent;

public class DeletePetFromEventCommand : IRequest
{
    public int EventId { get; set; }
    public int PetId { get; set; }
}
