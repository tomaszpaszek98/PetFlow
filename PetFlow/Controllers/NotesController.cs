using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Controllers;

public class NotesController : BaseController
{
    [HttpPost(ApiEndpoints.Pets.Notes.Create)]
    public IActionResult Create(int petId)
    {
        return Created(string.Empty, null);
    }

    [HttpGet(ApiEndpoints.Pets.Notes.Get)]
    public IActionResult Get(int petId, int id)
    {
        return Ok();
    }

    [HttpGet(ApiEndpoints.Pets.Notes.GetAll)]
    public IActionResult GetAll(int petId)
    {
        return Ok();
    }

    [HttpPut(ApiEndpoints.Pets.Notes.Update)]
    public IActionResult Update(int petId, int id)
    {
        return Ok();
    }

    [HttpDelete(ApiEndpoints.Pets.Notes.Delete)]
    public IActionResult Delete(int petId, int id)
    {
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Pets.Notes.Summary.Get)]
    public IActionResult GetSummary(int petId, int id)
    {
        return Ok();
    }
}
