using Application.Pets.Commands.CreatePet;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Pets.Commands.CreatePet;

public class CreatePetCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnPetResponseWhenPetIsCreatedSuccessfully()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new CreatePetCommand
        {
            Name = "Burek",
            Species = "Dog",
            Breed = "Mieszaniec",
            DateOfBirth = new DateTime(2020, 1, 1)
        };
        repository.CreateAsync(
            Arg.Is<Pet>(p => p.Name == command.Name && 
                             p.Species == command.Species &&
                             p.Breed == command.Breed &&
                             p.DateOfBirth == command.DateOfBirth),
            Arg.Any<CancellationToken>())
            .Returns(true);
        var handler = new CreatePetCommandHandler(repository);

        // WHEN
        var result = await handler.Handle(command);

        // THEN
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Species.Should().Be(command.Species);
        result.Breed.Should().Be(command.Breed);
        result.DateOfBirth.Should().Be(command.DateOfBirth);
    }

    [Test]
    public async Task ShouldThrowEntityCreationExceptionWhenPetIsNotCreated()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new CreatePetCommand
        {
            Name = "Burek",
            Species = "Dog",
            Breed = "Mieszaniec",
            DateOfBirth = new DateTime(2020, 1, 1)
        };
        repository.CreateAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>()).Returns(false);
        var handler = new CreatePetCommandHandler(repository);
        
        // WHEN
        var act = () => handler.Handle(command);
        
        // THEN
        await act.Should().ThrowAsync<EntityCreationException>();
    }
}
