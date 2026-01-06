using Application.Pets.Commands.CreatePet;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Controllers.Pets;

[Parallelizable(ParallelScope.Fixtures)]
public class CreatePetTests : BaseIntegrationTest
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

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var command = new CreatePetCommand
        {
            Name = string.Empty,
            Species = "Dog",
            Breed = "Golden Retriever",
            DateOfBirth = DateTime.UtcNow.AddYears(-3)
        };

        // WHEN
        var act = async () => await Sender.Send(command);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }

    [Test]
    public async Task ShouldCreateMultiplePetsWithUniqueIdsWhenMultipleCommandsAreSent()
    {
        // GIVEN
        var command1 = new CreatePetCommand
        {
            Name = "Max",
            Species = "Dog",
            Breed = "Labrador",
            DateOfBirth = DateTime.UtcNow.AddYears(-5)
        };

        var command2 = new CreatePetCommand
        {
            Name = "Whiskers",
            Species = "Cat",
            Breed = "Persian",
            DateOfBirth = DateTime.UtcNow.AddYears(-2)
        };

        // WHEN
        var result1 = await Sender.Send(command1);
        var result2 = await Sender.Send(command2);

        // THEN
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Id.Should().NotBe(result2.Id);
        result1.Name.Should().BeEquivalentTo("Max");
        result2.Name.Should().BeEquivalentTo("Whiskers");
    }

    [Test]
    public async Task ShouldCreatePetSuccessfullyWhenDateOfBirthIsToday()
    {
        // GIVEN
        var today = DateTime.UtcNow.Date;
        var command = new CreatePetCommand
        {
            Name = "Newborn",
            Species = "Dog",
            Breed = "Poodle",
            DateOfBirth = today
        };

        // WHEN
        var result = await Sender.Send(command);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.DateOfBirth.Should().Be(today);
    }
}