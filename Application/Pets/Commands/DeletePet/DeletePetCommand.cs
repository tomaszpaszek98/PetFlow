using MediatR;

namespace Application.Pets.Commands.DeletePet;

public record DeletePetCommand : IRequest
{
    public int PetId { get; init; }
}