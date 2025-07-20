using Application.Pets.Common;

namespace Application.Pets.Queries.GetPets;

public class PetsResponse
{
    public IEnumerable<PetResponse> Items { get; set; } = new List<PetResponse>();
}

