using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

[ApiController]
[Route("api/animals")]
[Produces("application/json")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _service;

    public AnimalController(IAnimalService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalDto>> Get(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AnimalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnimalDto>> Create(CreateAnimalRequest request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalDto>> Update(Guid id, UpdateAnimalRequest request)
        => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
