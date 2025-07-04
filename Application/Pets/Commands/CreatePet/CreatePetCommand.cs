using MediatR;

namespace Application.Pets.Commands.CreatePet;

public class CreatePetCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhotoUrl { get; set; }
}