using Microsoft.AspNetCore.Mvc;

namespace PetFlow.Controllers;

public class PetsController : BaseController
{
    [HttpPost(ApiEndpoints.Pets.Create)]
    public IActionResult Create()
    {
        return Created(string.Empty, null);
    }

    [HttpGet(ApiEndpoints.Pets.Get)]
    public IActionResult Get(int id)
    {
        return Ok();
    }

    [HttpGet(ApiEndpoints.Pets.GetAll)]
    public IActionResult GetAll()
    {
        return Ok();
    }

    [HttpPut(ApiEndpoints.Pets.Update)]
    public IActionResult Update(int id)
    {
        return Ok();
    }

    [HttpDelete(ApiEndpoints.Pets.Delete)]
    public IActionResult Delete(int id)
    {
        return NoContent();
    }
    
    [HttpPost(ApiEndpoints.Pets.Photo.Upload)]
    public IActionResult UploadPhoto()
    {
        return Created(string.Empty, null);
    }
    
    [HttpDelete(ApiEndpoints.Pets.Photo.Delete)]
    public IActionResult DeletePhoto(int id)
    {
        return NoContent();
    }
}
