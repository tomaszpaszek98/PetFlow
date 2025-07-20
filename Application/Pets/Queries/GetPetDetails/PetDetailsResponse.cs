using Application.Pets.Common;

namespace Application.Pets.Queries.GetPetDetails;

public class PetDetailsResponse : PetResponse
{
    public UpcomingEventResponse? UpcomingEvent { get; set; }
}