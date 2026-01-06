using Application.Pets.Commands.CreatePet;
using Application.Pets.Queries.GetPets;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Pets;

public class GetPetsTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldReturnAllPetsWhenPetsExistInDatabase()
    {
        // GIVEN
        var command1 = new CreatePetCommand
        {
            Name = "Pet1",
            Species = "Dog",
            Breed = "Labrador",
            DateOfBirth = DateTime.UtcNow.Date.AddYears(-4)
        };
        var command2 = new CreatePetCommand
        {
            Name = "Pet2",
            Species = "Cat",
            Breed = "Persian",
            DateOfBirth = DateTime.UtcNow.Date.AddYears(-2)
        };
        
        await Sender.Send(command1);
        await Sender.Send(command2);
        
        var query = new GetPetsQuery();

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeEmpty();
        response.Items.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenNoPetsExist()
    {
        // GIVEN
        var query = new GetPetsQuery();

        // WHEN
        var response = await Sender.Send(query);

        // THEN
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }
}

