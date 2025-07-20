using MediatR;

namespace Application.Pets.Commands.DeletePet;

public class DeletePetCommand : IRequest<bool>
{
    public int PetId { get; set; }
}

