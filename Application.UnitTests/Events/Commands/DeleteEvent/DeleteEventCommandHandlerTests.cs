using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.DeleteEvent;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UnitTests.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandlerTests
{
    [Test]
    public async Task ShouldCompleteSuccessfullyWhenEventIsDeleted()
    {
        // GIVEN
        var eventId = 1;
        var command = new DeleteEventCommand { EventId = eventId };
        var repository = Substitute.For<IEventRepository>();
        var handler = new DeleteEventCommandHandler(repository, Any.Instance<ILogger<DeleteEventCommandHandler>>());
        
        repository.DeleteAsync(eventId, Arg.Any<CancellationToken>()).Returns(true);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().NotThrowAsync();
        await repository.Received(1).DeleteAsync(eventId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var eventId = 99;
        var command = new DeleteEventCommand { EventId = eventId };
        var repository = Substitute.For<IEventRepository>();
        var handler = new DeleteEventCommandHandler(repository, Any.Instance<ILogger<DeleteEventCommandHandler>>());

        repository.DeleteAsync(eventId, Arg.Any<CancellationToken>()).Returns(false);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Event)) && e.Message.Contains(eventId.ToString()));
        await repository.Received(1).DeleteAsync(eventId, Arg.Any<CancellationToken>());
    }
}
