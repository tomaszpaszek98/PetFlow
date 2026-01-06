using Application.Pets.Queries.GetPetDetails;
using Domain.Entities;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Pets;

public class GetPetDetailsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnPetDetailsWithUpcomingEventWhenPetHasEvent()
    {
        // GIVEN
        var petId = 1; // Pet from seed data that has upcoming events
        var query = new GetPetDetailsQuery { PetId = petId };
        
        // WHEN
        var response = await Sender.Send(query);
        
        // THEN
        response.Should().NotBeNull();
        response.Id.Should().Be(petId);
        response.UpcomingEvent.Should().NotBeNull();
    }
    
    [Test]
    public async Task ShouldReturnPetDetailsWithoutUpcomingEventWhenPetExistsAndDoesNotHaveEvent()
    {
        // GIVEN
        var petId = 3; // Pet from seed data that has not upcoming events
        var query = new GetPetDetailsQuery { PetId = petId };
        
        // WHEN
        var response = await Sender.Send(query);
        
        // THEN
        response.Should().NotBeNull();
        response.Id.Should().Be(petId);
        response.UpcomingEvent.Should().BeNull();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var query = new GetPetDetailsQuery { PetId = nonExistentPetId };
        
        // WHEN
        var act = async () => await Sender.Send(query);
        
        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Pet)}*{nonExistentPetId}*");
    }
}