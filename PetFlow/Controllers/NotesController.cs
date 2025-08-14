using Application.Notes.Commands.DeleteNote;
using Application.Notes.Queries.GetNoteDetails;
using Application.Notes.Queries.GetNotes;
using Microsoft.AspNetCore.Mvc;
using PetFlow.Requests;
using PetFlow.Requests.Note;

namespace PetFlow.Controllers;

public class NotesController : BaseController
{
    [HttpPost(ApiEndpoints.Pets.Notes.Create)]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request,
        [FromRoute] int petId, CancellationToken cancellationToken)
    {
        var command = request.MapToCommand(petId);
        var result = await Mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet(ApiEndpoints.Pets.Notes.Get)]
    public async Task<IActionResult> Get([FromRoute] int petId, [FromRoute] int noteId, CancellationToken cancellationToken)
    {
        var query = new GetNoteDetailsQuery { PetId = petId, NoteId = noteId };
        var result = await Mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    [HttpGet(ApiEndpoints.Pets.Notes.GetAll)]
    public async Task<IActionResult> GetAll([FromRoute] int petId, CancellationToken cancellationToken)
    {
        var query = new GetNotesQuery { PetId = petId };
        var result = await Mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Pets.Notes.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateNoteRequest request,
        [FromRoute] int petId, [FromRoute] int noteId, CancellationToken cancellationToken)
    {
        var command = request.MapToCommand(petId, noteId);
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.Pets.Notes.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int petId, [FromRoute] int noteId, CancellationToken cancellationToken)
    {
        var command = new DeleteNoteCommand { PetId = petId, NoteId = noteId };
        await Mediator.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Pets.Notes.Summary.GetForNote)]
    public IActionResult GetSummary([FromRoute] int petId, [FromRoute] int noteId, CancellationToken cancellationToken)
    {
        //TODO to implement
        return Ok();
    }
    
    [HttpGet(ApiEndpoints.Pets.Notes.Summary.GetForTimeRange)]
    public IActionResult GetSummaryForTimeRange([FromRoute] int petId, CancellationToken cancellationToken)
    {
        //TODO to implement
        return Ok();
    }
}
