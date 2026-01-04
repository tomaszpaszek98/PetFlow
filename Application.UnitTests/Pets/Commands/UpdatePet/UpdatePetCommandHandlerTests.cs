using Application.Common.Interfaces.Repositories;
using Application.Pets.Commands.UpdatePet;
using Application.Pets.Common;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.Pets.Commands.UpdatePet;

public class UpdatePetCommandHandlerTests
{
    [Test]
    public async Task ShouldUpdatePetAndReturnPetResponseWhenPetExists()
    {
        // GIVEN
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
        var repository = Substitute.For<IPetRepository>();
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
        result.Should().BeOfType<PetResponse>();
        result.Id.Should().Be(command.Id);
        result.Name.Should().Be(command.Name);
        result.Species.Should().Be(command.Species);
        result.Breed.Should().Be(command.Breed);
        result.DateOfBirth.Should().Be(command.DateOfBirth);
        
        Received.InOrder(() => {
            repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            repository.UpdateAsync(Arg.Is<Pet>(p => 
                p.Id == command.Id && 
                p.Name == command.Name && 
                p.Species == command.Species && 
                p.Breed == command.Breed && 
                p.DateOfBirth == command.DateOfBirth), 
                Arg.Any<CancellationToken>());
        });
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenPetDoesNotExist()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 99,
            Name = "Reksio",
            Species = "Dog",
            Breed = "Chichuahua",
            DateOfBirth = new DateTime(2019, 5, 5)
        };
        var repository = Substitute.For<IPetRepository>();
        var handler = new UpdatePetCommandHandler(repository);
        repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Pet)null);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>();
        await repository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await repository.DidNotReceive().UpdateAsync(default);
    }

    [Test]
    public async Task ShouldUpdatePetNameWhenOnlyNameIsChanged()
    {
        // GIVEN
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "NewName",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = new DateTime(2019, 5, 5)
        };
        var existingPet = new Pet
        {
            Id = 1,
            Name = "OldName",
            Species = "Dog",
            Breed = "Chihuahua",
            DateOfBirth = new DateTime(2019, 5, 5)
        };
        var repository = Substitute.For<IPetRepository>();
        var handler = new UpdatePetCommandHandler(repository);

        repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingPet);
        repository.UpdateAsync(
            Arg.Is<Pet>(p => p.Id == command.Id && 
                             p.Name == "NewName" && 
                             p.Species == "Dog" && 
                             p.Breed == "Chihuahua" && 
                             p.DateOfBirth == new DateTime(2019, 5, 5)),
            Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Name.Should().Be("NewName");
        result.Species.Should().Be("Dog");
        
        Received.InOrder(() => {
            repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            repository.UpdateAsync(Arg.Is<Pet>(p => p.Name == "NewName"), Arg.Any<CancellationToken>());
        });
    }

    [Test]
    public async Task ShouldUpdatePetDateOfBirthWhenDateOfBirthIsChanged()
    {
        // GIVEN
        var newDateOfBirth = new DateTime(2018, 3, 3);
        var command = new UpdatePetCommand
        {
            Id = 1,
            Name = "Rex",
            Species = "Dog",
            Breed = "Labrador",
            DateOfBirth = newDateOfBirth
        };
        var existingPet = new Pet
        {
            Id = 1,
            Name = "Rex",
            Species = "Dog",
            Breed = "Labrador",
            DateOfBirth = new DateTime(2020, 1, 1)
        };
        var repository = Substitute.For<IPetRepository>();
        var handler = new UpdatePetCommandHandler(repository);

        repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(existingPet);
        repository.UpdateAsync(
            Arg.Is<Pet>(p => p.Id == command.Id && 
                             p.DateOfBirth == newDateOfBirth),
            Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        
        // WHEN
        var result = await handler.Handle(command, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Name.Should().Be(existingPet.Name);
        result.DateOfBirth.Should().Be(newDateOfBirth);
        
        Received.InOrder(() => {
            repository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
            repository.UpdateAsync(Arg.Is<Pet>(p => p.DateOfBirth == newDateOfBirth), Arg.Any<CancellationToken>());
        });
    }
}
