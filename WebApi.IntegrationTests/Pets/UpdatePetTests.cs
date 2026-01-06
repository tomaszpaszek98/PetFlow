using Application.Pets.Commands.UpdatePet;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Pets;

public class UpdatePetTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldUpdatePetWhenPetExistsAndCommandIsValid()
    {
        // GIVEN
        var petId = 1;
        var updatedName = "UpdatedName";
        var updatedBreed = "Updated Breed";
        var updateCommand = new UpdatePetCommand
        {
            Id = petId,
            Name = updatedName,
            Species = "Dog",
            Breed = updatedBreed,
            DateOfBirth = DateTime.UtcNow.Date.AddYears(-3)
        };

        // WHEN
        var result = await Sender.Send(updateCommand);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(petId);
        result.Name.Should().Be(updatedName);
        result.Breed.Should().Be(updatedBreed);
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var updateCommand = new UpdatePetCommand
        {
            Id = nonExistentPetId,
            Name = "UpdatedName",
            Species = "Dog",
            Breed = "Breed",
            DateOfBirth = DateTime.UtcNow.Date.AddYears(-2)
        };

        // WHEN
        var act = async () => await Sender.Send(updateCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Pet)}*{nonExistentPetId}*");
    }

    [Test]
    public async Task ShouldThrowValidationExceptionWhenCommandIsInvalid()
    {
        // GIVEN
        var petId = 1;
        var invalidUpdateCommand = new UpdatePetCommand
        {
            Id = petId,
            Name = string.Empty,
            Species = "Cat",
            Breed = "Siamese",
            DateOfBirth = DateTime.UtcNow.Date.AddYears(-1)
        };

        // WHEN
        var act = async () => await Sender.Send(invalidUpdateCommand);

        // THEN
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any());
    }
}

