using Application.Pets.Queries.GetPets;
using Domain.Entities;
using Persistance.Repositories;

namespace Application.UnitTests.Pets.Queries.GetPets;

[TestFixture]
public class GetPetsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnPetsResponseWithAllPets()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var pets = new List<Pet>
        {
            new() { Id = 1, Name = "Denny", Species = "Dog", Breed = "Chihuahua", DateOfBirth = new DateTime(2020, 1, 1) },
            new() { Id = 2, Name = "Milo", Species = "Cat", Breed = "Siamese", DateOfBirth = new DateTime(2019, 5, 5) }
        };
        var handler = new GetPetsQueryHandler(repository);
        var query = new GetPetsQuery();
        
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(pets);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Select(x => x.Id).Should().BeEquivalentTo(pets.Select(x => x.Id));
    }

    [Test]
    public async Task ShouldReturnEmptyPetsResponseWhenNoPetsExist()
    {
        // GIVEN
        var repository = Substitute.For<IPetRepository>();
        var handler = new GetPetsQueryHandler(repository);
        var query = new GetPetsQuery();
        
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Pet>());

        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }
}

