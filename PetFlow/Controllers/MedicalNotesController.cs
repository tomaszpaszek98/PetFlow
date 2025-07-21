using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Controllers;

public class MedicalNotesController : BaseController
{
    [HttpPost(ApiEndpoints.MedicalNotes.Create)]
    public IActionResult Create(int petId)
    {
        return Created(string.Empty, null);
    }

    [HttpGet(ApiEndpoints.MedicalNotes.Get)]
    public IActionResult Get(int petId, int id)
    {
        return Ok();
    }

    [HttpGet(ApiEndpoints.MedicalNotes.GetAll)]
    public IActionResult GetAll(int petId)
    {
        return Ok();
    }

    [HttpPut(ApiEndpoints.MedicalNotes.Update)]
    public IActionResult Update(int petId, int id)
    {
        return Ok();
    }

    [HttpDelete(ApiEndpoints.MedicalNotes.Delete)]
    public IActionResult Delete(int petId, int id)
    {
        return NoContent();
    }

    [HttpGet(ApiEndpoints.MedicalNotes.GetSummary)]
    public IActionResult GetSummary(int petId, int id)
    {
        return Ok();
    }
}
