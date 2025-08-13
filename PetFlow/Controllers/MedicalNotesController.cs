using Application.MedicalNotes.Commands.DeleteMedicalNote;
using Application.MedicalNotes.Queries.GetMedicalNoteDetails;
using Application.MedicalNotes.Queries.GetMedicalNotes;
using Microsoft.AspNetCore.Mvc;
using PetFlow.Requests;
using PetFlow.Requests.MedicalNote;

namespace PetFlow.Controllers;

public class MedicalNotesController : BaseController
{
    [HttpPost(ApiEndpoints.Pets.MedicalNotes.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMedicalNoteRequest request, [FromRoute] int petId)
    {
        var command = request.MapToCommand(petId);
        var result = await Mediator.Send(command);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Get)]
    public async Task<IActionResult> Get([FromRoute] int petId, [FromRoute] int medicalNoteId)
    {
        var result = await Mediator.Send(new GetMedicalNoteDetailsQuery{ PetId = petId, MedicalNoteId = medicalNoteId});
        
        return Ok(result);
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.GetAll)]
    public async Task<IActionResult> GetAll([FromRoute] int petId)
    {
        var result = await Mediator.Send(new GetMedicalNotesQuery { PetId = petId });
        
        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Pets.MedicalNotes.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateMedicalNoteRequest request, [FromRoute] int petId, [FromRoute] int medicalNoteId)
    {
        var command = request.MapToCommand(petId, medicalNoteId);
        var result = await Mediator.Send(command);
        
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.Pets.MedicalNotes.Delete)]
    public async Task<IActionResult> Delete(int petId, int id)
    {
        await Mediator.Send(new DeleteMedicalNoteCommand { PetId = petId, MedicalNoteId = id });
        
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Summary.GetForNote)]
    public IActionResult GetSummary(int petId, int id)
    {
        //TODO to implement
        return Ok();
    }
    
    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Summary.GetForTimeRange)]
    public IActionResult GetSummaryForTimeRange(int petId)
    {
        //TODO to implement
        return Ok();
    }
}
