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
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest request, [FromRoute] int petId)
    {
        var command = request.MapToCommand(petId);
        var result = await Mediator.Send(command);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet(ApiEndpoints.Pets.Notes.Get)]
    public async Task<IActionResult> Get([FromRoute] int petId, [FromRoute] int noteId)
    {
        var result = await Mediator.Send(new GetNoteDetailsQuery { PetId = petId, NoteId = noteId });
        
        return Ok(result);
    }

    [HttpGet(ApiEndpoints.Pets.Notes.GetAll)]
    public async Task<IActionResult> GetAll([FromRoute] int petId)
    {
        var result = await Mediator.Send(new GetNotesQuery { PetId = petId });
        
        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Pets.Notes.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateNoteRequest request,
        [FromRoute] int petId, [FromRoute] int noteId)
    {
        var command = request.MapToCommand(petId, noteId);
        var result = await Mediator.Send(command);
        
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.Pets.Notes.Delete)]
    public async Task<IActionResult> Delete(int petId, int noteId)
    {
        await Mediator.Send(new DeleteNoteCommand { PetId = petId, NoteId = noteId });
        
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Pets.Notes.Summary.GetForNote)]
    public IActionResult GetSummary(int petId, int id)
    {
        //TODO to implement
        return Ok();
    }
    
    [HttpGet(ApiEndpoints.Pets.Notes.Summary.GetForTimeRange)]
    public IActionResult GetSummaryForTimeRange(int petId)
    {
        //TODO to implement
        return Ok();
    }
}
