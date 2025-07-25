using Application.Pets.Common;

namespace Application.Pets.Queries.GetPetDetails;

public class PetDetailsResponse : PetResponse
{
    public UpcomingEventResponse? UpcomingEvent { get; set; }
}

public class UpcomingEventResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime EventDate { get; set; }
}