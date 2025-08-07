using Application.Pets.Commands.DeletePet;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Pets.Commands.DeletePet;

public class DeletePetCommandHandlerTests
{
    [Test]
    public async Task ShouldCompleteSuccessfullyWhenPetIsDeleted()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new DeletePetCommand { PetId = 1 };
        var handler = new DeletePetCommandHandler(repository);
        
        repository.DeleteByIdAsync(command.PetId, Arg.Any<CancellationToken>()).Returns(true);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().NotThrowAsync();
        await repository.Received(1).DeleteByIdAsync(command.PetId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowEntityNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new DeletePetCommand { PetId = 99 };
        var handler = new DeletePetCommandHandler(repository);

        repository.DeleteByIdAsync(command.PetId, Arg.Any<CancellationToken>()).Returns(false);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
        await repository.Received(1).DeleteByIdAsync(command.PetId, Arg.Any<CancellationToken>());
    }
}
