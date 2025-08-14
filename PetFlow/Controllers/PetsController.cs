using Application.Pets.Commands.CreatePet;
using Application.Pets.Commands.DeletePet;
using Application.Pets.Queries.GetPetDetails;
using Application.Pets.Queries.GetPetEvents;
using Application.Pets.Queries.GetPets;
using Microsoft.AspNetCore.Mvc;
using PetFlow.Requests;
using PetFlow.Requests.Pet;

namespace PetFlow.Controllers;

public class PetsController : BaseController
{
    [HttpPost(ApiEndpoints.Pets.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePetCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpGet(ApiEndpoints.Pets.Get)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPetDetailsQuery { PetId = id }, cancellationToken);
        
        return Ok(result);
    }

    [HttpGet(ApiEndpoints.Pets.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPetsQuery(), cancellationToken);
        
        return Ok(result);
    }

    [HttpPut(ApiEndpoints.Pets.Update)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePetRequest request, CancellationToken cancellationToken)
    {
        var command = request.MapToCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    [HttpDelete(ApiEndpoints.Pets.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePetCommand { PetId = id  }, cancellationToken);
        
        return NoContent();
    }
    
    [HttpPut(ApiEndpoints.Pets.Photo.Upload)]
    public IActionResult UploadPhoto()
    {
        //TODO to implement after making POC
        return Accepted(string.Empty, null);
    }
    
    [HttpDelete(ApiEndpoints.Pets.Photo.Delete)]
    public IActionResult DeletePhoto([FromRoute] int id)
    {
        //TODO to implement after making POC
        return NoContent();
    }

    [HttpGet(ApiEndpoints.Pets.Events.GetAll)]
    public async Task<IActionResult> GetAllPetEvents([FromRoute] int petId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPetEventsQuery { PetId = petId}, cancellationToken);
        
        return Ok(result);
    }
}
