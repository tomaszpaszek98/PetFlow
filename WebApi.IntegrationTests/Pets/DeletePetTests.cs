using Application.Pets.Commands.DeletePet;
using Domain.Entities;
using Domain.Exceptions;
using WebApi.IntegrationTests.Common;

namespace WebApi.IntegrationTests.Pets;

public class DeletePetTests : BaseIntegrationTest
{
    [Test]
    public async Task ShouldDeletePetWhenPetExists()
    {
        // GIVEN
        var petId = 1;
        var deleteCommand = new DeletePetCommand { PetId = petId };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var nonExistentPetId = 999999;
        var deleteCommand = new DeletePetCommand { PetId = nonExistentPetId };

        // WHEN
        var act = async () => await Sender.Send(deleteCommand);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{nameof(Pet)}*{nonExistentPetId}*");
    }
}

