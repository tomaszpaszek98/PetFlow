using Application.Pets.Common;
using MediatR;

namespace Application.Pets.Commands.CreatePet;

public class CreatePetCommand : IRequest<PetResponse>
{
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }

    public string ToString()
    {
        return $"Name = {Name}, Species = {Species}, Breed = {Breed}, DateOfBirth = {DateOfBirth}";
    }
}