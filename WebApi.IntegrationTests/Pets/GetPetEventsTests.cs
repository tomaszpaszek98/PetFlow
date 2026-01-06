using Application.Pets.Queries.GetPetEvents;
using Domain.Entities;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Pets;

public class GetPetEventsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnPetEventsWhenPetExists()
    {
        // GIVEN
        var petId = 1; // Pet from seed data
        var query = new GetPetEventsQuery { PetId = petId };

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var query = new GetPetEventsQuery { PetId = nonExistentPetId };

        // WHEN
        var act = async () => await Sender.Send(query);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Pet)}*{nonExistentPetId}*");
    }
}

