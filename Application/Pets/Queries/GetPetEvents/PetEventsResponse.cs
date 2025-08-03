namespace Application.Pets.Queries.GetPetEvents;

public class PetEventsResponse
{
    public IEnumerable<PetEventResponse> Items { get; set; } = new List<PetEventResponse>();
}

public class PetEventResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime DateOfEvent { get; set; }
    public bool Reminder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}