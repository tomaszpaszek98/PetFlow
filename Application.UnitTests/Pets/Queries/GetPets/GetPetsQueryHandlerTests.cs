using Application.Common.Interfaces.Repositories;
using Application.Pets.Queries.GetPets;
using Domain.Entities;

namespace Application.UnitTests.Pets.Queries.GetPets;

public class GetPetsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnPetsResponseWithAllPets()
    {
        // GIVEN
        var pets = new List<Pet>
        {
            new() { Id = 1, Name = "Denny", Species = "Dog", Breed = "Chihuahua", DateOfBirth = new DateTime(2020, 1, 1) },
            new() { Id = 2, Name = "Milo", Species = "Cat", Breed = "Siamese", DateOfBirth = new DateTime(2019, 5, 5) }
        };
        var repository = Substitute.For<IPetRepository>();
        var handler = new GetPetsQueryHandler(repository);
        var query = new GetPetsQuery();
        
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(pets);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Select(x => x.Id).Should().BeEquivalentTo(pets.Select(x => x.Id));
        await repository.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldReturnEmptyPetsResponseWhenNoPetsExist()
    {
        // GIVEN
        var query = new GetPetsQuery();
        var repository = Substitute.For<IPetRepository>();
        var handler = new GetPetsQueryHandler(repository);
        
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Pet>());

        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        await repository.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
    }
}