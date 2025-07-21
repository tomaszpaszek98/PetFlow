using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Controllers;

public class NotesController : BaseController
{
    [HttpPost(ApiEndpoints.Notes.Create)]
    public IActionResult Create(int petId)
    {
        return Created(string.Empty, null);
    }

    [HttpGet(ApiEndpoints.Notes.Get)]
    public IActionResult Get(int petId, int id)
    {
        return Ok();
    }

    [HttpGet(ApiEndpoints.Notes.GetAll)]
    public IActionResult GetAll(int petId)
    {
        return Ok();
    }

    [HttpPut(ApiEndpoints.Notes.Update)]
    public IActionResult Update(int petId, int id)
    {
        return Ok();
    }

    [HttpDelete(ApiEndpoints.Notes.Delete)]
    public IActionResult Delete(int petId, int id)
    {
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Notes.GetSummary)]
    public IActionResult GetSummary(int petId, int id)
    {
        return Ok();
    }
}
