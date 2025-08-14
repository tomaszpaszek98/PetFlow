using Application.Events.Commands.AddPetToEvent;
using Application.Events.Commands.CreateEvent;
using Application.Events.Commands.DeleteEvent;
using Application.Events.Commands.DeletePetFromEvent;
using Application.Events.Commands.UpdateEvent;
using Application.Events.Queries.GetEventDetails;
using Application.Events.Queries.GetEvents;
using Microsoft.AspNetCore.Mvc;
using PetFlow.Requests;
using PetFlow.Requests.Event;

namespace PetFlow.Controllers;

[ApiController]
public class EventsController : BaseController
{
    [HttpPost(ApiEndpoints.Events.Create)]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet(ApiEndpoints.Events.Get)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetEventDetailsQuery { EventId = id };
        var result = await Mediator.Send(query, cancellationToken);
        
        return Ok(result);
    } 

    [HttpGet(ApiEndpoints.Events.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetEventsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Events.Update)]
    public async Task<IActionResult> Update([FromRoute] int id,
        [FromBody] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.Events.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteEventCommand { EventId = id };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
    
    [HttpPost(ApiEndpoints.Events.Pets.Add)]
    public async Task<IActionResult> AddPet([FromBody] AddPetToEventCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { Id = result.EventId });
    }

    [HttpDelete(ApiEndpoints.Events.Pets.Delete)]
    public async Task<IActionResult> DeletePet([FromRoute] int eventId, [FromRoute] int petId,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetFromEventCommand { EventId = eventId, PetId = petId };
        await Mediator.Send(command, cancellationToken);
        
        return NoContent();
    }
}
