using Application.Pets.Commands.CreatePet;
using Domain.Entities;
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
            Name = "Denny",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = new DateTime(2020, 1, 1)
        };
        var createdPet = new Pet
        {
            //TODO add Id property to Pet entity here after implementing
            //the repository with EF Core
            Name = command.Name,
            Species = command.Species,
            Breed = command.Breed,
            DateOfBirth = command.DateOfBirth
        };
        var handler = new CreatePetCommandHandler(repository);
        repository.CreateAsync(
                Arg.Is<Pet>(p => p.Name == command.Name &&
                                 p.Species == command.Species &&
                                 p.Breed == command.Breed &&
                                 p.DateOfBirth == command.DateOfBirth),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(createdPet));
        
        // WHEN
        var result = await handler.Handle(command);

        // THEN
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Species.Should().Be(command.Species);
        result.Breed.Should().Be(command.Breed);
        result.DateOfBirth.Should().Be(command.DateOfBirth);
        
        await repository.Received(1).CreateAsync(
            Arg.Is<Pet>(p => p.Name == command.Name &&
                            p.Species == command.Species &&
                            p.Breed == command.Breed &&
                            p.DateOfBirth == command.DateOfBirth),
            Arg.Any<CancellationToken>());
    }
}

