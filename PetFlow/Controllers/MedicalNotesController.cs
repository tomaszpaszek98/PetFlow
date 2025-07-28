using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Controllers;

public class MedicalNotesController : BaseController
{
    [HttpPost(ApiEndpoints.Pets.MedicalNotes.Create)]
    public IActionResult Create(int petId)
    {
        return Created(string.Empty, null);
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Get)]
    public IActionResult Get(int petId, int id)
    {
        return Ok();
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.GetAll)]
    public IActionResult GetAll(int petId)
    {
        return Ok();
    }

    [HttpPut(ApiEndpoints.Pets.MedicalNotes.Update)]
    public IActionResult Update(int petId, int id)
    {
        return Ok();
    }

    [HttpDelete(ApiEndpoints.Pets.MedicalNotes.Delete)]
    public IActionResult Delete(int petId, int id)
    {
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Summary.Get)]
    public IActionResult GetSummary(int petId, int id)
    {
        return Ok();
    }
}
