using Application.Pets.Commands.DeletePet;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Pets.Commands.DeletePet;

public class DeletePetCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnTrueWhenPetIsDeletedSuccessfully()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new DeletePetCommand { PetId = 1 };
        repository.DeleteByIdAsync(command.PetId, Arg.Any<CancellationToken>()).Returns(true);
        var handler = new DeletePetCommandHandler(repository);

        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);

        // THEN
        result.Should().BeTrue();
    }

    [Test]
    public async Task ShouldThrowEntityNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new DeletePetCommand { PetId = 99 };
        repository.DeleteByIdAsync(command.PetId, Arg.Any<CancellationToken>()).Returns(false);
        var handler = new DeletePetCommandHandler(repository);

        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
    }
}

