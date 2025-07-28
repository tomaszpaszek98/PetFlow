using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Controllers;

[ApiController]
public class EventsController : BaseController
{
    [HttpPost(ApiEndpoints.Events.Create)]
    public IActionResult Create(int petId)
    {
        return Created(string.Empty, null);
    }

    [HttpGet(ApiEndpoints.Events.Get)]
    public IActionResult Get(int petId, int id)
    {
        return Ok();
    }

    [HttpGet(ApiEndpoints.Events.GetAll)]
    public IActionResult GetAll(int petId)
    {
        return Ok();
    }

    [HttpPut(ApiEndpoints.Events.Update)]
    public IActionResult Update(int petId, int id)
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
