using Application.Common.Interfaces.Repositories;
using Application.Events.Queries.GetEvents;
using Domain.Entities;

namespace Application.UnitTests.Events.Queries.GetEvents;

public class GetEventsQueryHandlerTests
{
    [Test]
    public async Task ShouldReturnEventsResponseWithAllEventsWhenEventsExists()
    {
        // GIVEN
        var repository = Substitute.For<IEventRepository>();
        var events = new List<Event>
        {
            new() { Id = 1, Title = "Vet Visit", Description = "Regular checkup", DateOfEvent = new DateTime(2025, 8, 10), Reminder = true, Created = new DateTime(2025, 8, 1), Modified = new DateTime(2025, 8, 2) },
            new() { Id = 2, Title = "Vaccination", Description = "Annual vaccination", DateOfEvent = new DateTime(2025, 9, 15), Reminder = false, Created = new DateTime(2025, 8, 1), Modified = new DateTime(2025, 8, 3) }
        };
        var query = new GetEventsQuery();
        var handler = new GetEventsQueryHandler(repository);
        
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(events);
        
        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Select(x => x.Id).Should().BeEquivalentTo(events.Select(x => x.Id));
    }

    [Test]
    public async Task ShouldReturnEmptyEventsResponseWhenEventsDoesNotExist()
    {
        // GIVEN
        var repository = Substitute.For<IEventRepository>();
        var query = new GetEventsQuery();
        var handler = new GetEventsQueryHandler(repository);
        
        repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Event>());

        // WHEN
        var result = await handler.Handle(query, CancellationToken.None);

        // THEN
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
    }
}
