using Application.Pets.Commands.UpdatePet;

namespace PetFlow.Requests.Pet;

public static class RequestsMappingExtensions
{
    public static UpdatePetCommand MapToCommand(this UpdatePetRequest request, int id)
    {
        return new UpdatePetCommand
        {
            Id = id,
            Name = request.Name,
            Species = request.Species,
            Breed = request.Breed,
            DateOfBirth = request.DateOfBirth
        };
    }
}