using Application.Common.Interfaces.Repositories;
using Application.Pets.Commands.CreatePet;
using AutoFixture;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Pets.Commands.CreatePet;

public class CreatePetCommandHandlerTests
{
    [Test]
    public async Task ShouldReturnPetResponseWhenPetIsCreatedSuccessfully()
    {
        // GIVEN
        var createdPetId = 1;
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
            Id = createdPetId,
            Name = command.Name,
            Species = command.Species,
            Breed = command.Breed,
            DateOfBirth = command.DateOfBirth
        };
        var handler = new CreatePetCommandHandler(repository, Any.Instance<ILogger<CreatePetCommandHandler>>());
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
        result.Id.Should().Be(createdPetId);
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
    
    [Test]
    public async Task ShouldLogSensitiveDetailsAtDebugLevelWhenHandlingCreatePetCommand()
    {
        // GIVEN
        var name = "Denny";
        var species = "Dog";
        var breed = "Sensitive Breed Info";
        var dateOfBirth = new DateTime(2020, 1, 1);
        var petId = 10;
        var command = new CreatePetCommand
        {
            Name = name,
            Species = species,
            Breed = breed,
            DateOfBirth = dateOfBirth
        };
        var createdPet = new Pet
        {
            Id = petId,
            Name = name,
            Species = species,
            Breed = breed,
            DateOfBirth = dateOfBirth
        };
        var repository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<CreatePetCommandHandler>>();
        var handler = new CreatePetCommandHandler(repository, logger);
        
        repository.CreateAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>())
            .Returns(createdPet);
        
        // WHEN
        await handler.Handle(command);
        
        // THEN
        logger.Received(1).Log(
            LogLevel.Debug,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(breed)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
    
    [Test]
    public async Task ShouldNotLogSensitiveDetailsAtInformationLevelWhenHandlingCreatePetCommand()
    {
        // GIVEN
        var name = "Denny";
        var species = "Dog";
        var breed = "Sensitive Breed Info";
        var dateOfBirth = new DateTime(2020, 1, 1);
        var petId = 10;
        var command = new CreatePetCommand
        {
            Name = name,
            Species = species,
            Breed = breed,
            DateOfBirth = dateOfBirth
        };
        var createdPet = new Pet
        {
            Id = petId,
            Name = name,
            Species = species,
            Breed = breed,
            DateOfBirth = dateOfBirth
        };
        var repository = Substitute.For<IPetRepository>();
        var logger = Substitute.For<ILogger<CreatePetCommandHandler>>();
        var handler = new CreatePetCommandHandler(repository, logger);
        
        repository.CreateAsync(Arg.Any<Pet>(), Arg.Any<CancellationToken>())
            .Returns(createdPet);
        
        // WHEN
        await handler.Handle(command);
        
        // THEN
        logger.DidNotReceive().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains(breed)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
