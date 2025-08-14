using MediatR;

namespace Application.Pets.Commands.DeletePet;

public class DeletePetCommand : IRequest
{
    public int PetId { get; set; }
}