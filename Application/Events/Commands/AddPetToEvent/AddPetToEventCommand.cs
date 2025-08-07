using MediatR;

namespace Application.Events.Commands.AddPetToEvent;

public class AddPetToEventCommand : IRequest<AddPetToEventResponse>
{
    public int EventId { get; set; }
    public int PetId { get; set; }
}