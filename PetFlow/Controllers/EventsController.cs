using Application.Events.Commands.CreateEvent;
using Application.Events.Queries.GetEventDetails;
using Application.Events.Queries.GetEvents;
using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Controllers;

[ApiController]
public class EventsController : BaseController
{
    [HttpPost(ApiEndpoints.Events.Create)]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
    {
        var result = await Mediator.Send(command);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet(ApiEndpoints.Events.Get)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await Mediator.Send(new GetEventDetailsQuery { EventId = id });
        
        return Ok(result);
    }

    [HttpGet(ApiEndpoints.Events.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetEventsQuery());

        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Events.Update)]
    public async Task<IActionResult> Update(int id)
    {
        return Ok();
    }

    [HttpDelete(ApiEndpoints.Events.Delete)]
    public IActionResult Delete(int petId, int id)
    {
        return NoContent();
    }
    
    [HttpPost(ApiEndpoints.Events.Pets.Add)]
    public IActionResult AddPet()
    {
        return Created(string.Empty, null);
    }

    [HttpDelete(ApiEndpoints.Events.Pets.Delete)]
    public IActionResult DeletePet(int id)
    {
        return NoContent();
    }
}
