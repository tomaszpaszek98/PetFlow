using Application.Pets.Commands.UpdatePet;
using Domain.Entities;
using Domain.Exceptions;
using Persistance.Repositories;

namespace Application.UnitTests.Pets.Commands.UpdatePet;

public class UpdatePetCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnPetResponseWhenPetIsUpdatedSuccessfully()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Reksio",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = new DateTime(2019, 5, 5)
        };
        var pet = new Pet
        {
            Id = 1,
            Name = "OldName",
            Species = "OldSpecies",
            Breed = "OldBreed",
            DateOfBirth = new DateTime(2018, 1, 1)
        };
        var handler = new UpdatePetCommandHandler(repository);

        repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(pet);
        repository.UpdateAsync(
            Arg.Is<Pet>(p => p.Id == command.Id && 
                             p.Name == command.Name && 
                             p.Species == command.Species && 
                             p.Breed == command.Breed && 
                             p.DateOfBirth == command.DateOfBirth),
            Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Name.Should().Be(command.Name);
        result.Species.Should().Be(command.Species);
        result.Breed.Should().Be(command.Breed);
        result.DateOfBirth.Should().Be(command.DateOfBirth);
    }

    [Test]
    public async Task ShouldThrowEntityNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var command = new UpdatePetCommand
        {
            Id = 99,
            Name = "Reksio",
            Species = "Dog",
            Breed = "Chichuahua",
            DateOfBirth = new DateTime(2019, 5, 5)
        };
        var handler = new UpdatePetCommandHandler(repository);
        repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
