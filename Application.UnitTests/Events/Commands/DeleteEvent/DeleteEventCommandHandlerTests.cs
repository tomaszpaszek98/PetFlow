using Application.Common.Interfaces.Repositories;
using Application.Events.Commands.DeleteEvent;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UnitTests.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandlerTests
{
    [Test]
    public async Task ShouldCompleteSuccessfullyWhenEventIsDeleted()
    {
        // GIVEN
        var command = new DeleteEventCommand { EventId = 1 };
        var repository = Substitute.For<IEventRepository>();
        var handler = new DeleteEventCommandHandler(repository);
        
        repository.DeleteByIdAsync(command.EventId, Arg.Any<CancellationToken>()).Returns(true);
        
        // WHEN
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().NotThrowAsync();
        await repository.Received(1).DeleteByIdAsync(command.EventId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenEventDoesNotExist()
    {
        // GIVEN
        var command = new DeleteEventCommand { EventId = 99 };
        var repository = Substitute.For<IEventRepository>();
        var handler = new DeleteEventCommandHandler(repository);

        repository.DeleteByIdAsync(command.EventId, Arg.Any<CancellationToken>()).Returns(false);
        
        // WHEN
        var act = () => handler.Handle(command, CancellationToken.None);

        // THEN
        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Contains(nameof(Event)) && e.Message.Contains(command.EventId.ToString()));
        await repository.Received(1).DeleteByIdAsync(command.EventId, Arg.Any<CancellationToken>());
    }
}
