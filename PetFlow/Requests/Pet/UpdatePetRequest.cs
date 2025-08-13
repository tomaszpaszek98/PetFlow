namespace PetFlow.Requests.Pet;

public class UpdatePetRequest
{
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }
}