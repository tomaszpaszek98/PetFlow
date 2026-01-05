using Application.Pets.Commands.CreatePet;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Controllers.Pets;

[TestFixture]
public class GetDetailsTests : BaseIntegrationTest
{

    [Test]
    public async Task ShouldCreatePetWhenCreatePetCommandIsProvided()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = "Buddy",
            Species = "Dog",
            Breed = "Golden Retriever",
            DateOfBirth = DateTime.UtcNow.AddYears(-3)
        };
        
        // WHEN
        var result = await Sender.Send(command);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().BeEquivalentTo(command.Name);
        result.Species.Should().BeEquivalentTo(command.Species);
        result.Breed.Should().BeEquivalentTo(command.Breed);
        result.DateOfBirth.Should().Be(command.DateOfBirth);
    }
}