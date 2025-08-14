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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateMedicalNoteRequest request,
        [FromRoute] int petId, CancellationToken cancellationToken)
    {
        var command = request.MapToCommand(petId);
        var result = await Mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Get)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromRoute] int petId, [FromRoute] int medicalNoteId,
        CancellationToken cancellationToken)
    {
        var query = new GetMedicalNoteDetailsQuery { PetId = petId, MedicalNoteId = medicalNoteId };
        var result = await Mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromRoute] int petId, CancellationToken cancellationToken)
    {
        var query = new GetMedicalNotesQuery { PetId = petId };
        var result = await Mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Pets.MedicalNotes.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateMedicalNoteRequest request, 
        [FromRoute] int petId, [FromRoute] int medicalNoteId, CancellationToken cancellationToken)
    {
        var command = request.MapToCommand(petId, medicalNoteId);
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.Pets.MedicalNotes.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] int petId, [FromRoute] int medicalNoteId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteMedicalNoteCommand { PetId = petId, MedicalNoteId = medicalNoteId };
        await Mediator.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Summary.GetForNote)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetSummary([FromRoute] int petId, [FromRoute] int medicalNoteId,
        CancellationToken cancellationToken)
    {
        //TODO to implement
        return Ok();
    }
    
    [HttpGet(ApiEndpoints.Pets.MedicalNotes.Summary.GetForTimeRange)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetSummaryForTimeRange([FromRoute] int petId, CancellationToken cancellationToken)
    {
        //TODO to implement
        return Ok();
    }
}
