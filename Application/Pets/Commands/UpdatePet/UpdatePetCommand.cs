using Application.Pets.Common;
using MediatR;
using System;

namespace Application.Pets.Commands.UpdatePet;

public class UpdatePetCommand : IRequest<PetResponse>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }
}

