using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

[Route("api/animals")]
public class AnimalController : ApiControllerBase
{
    private readonly IAnimalService _service;

    public AnimalController(IAnimalService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse<AnimalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<CollectionResponse<AnimalDto>>> GetAll()
    {
        var animals = await _service.GetAllAsync();
        return Ok(new CollectionResponse<AnimalDto>
        {
            Items = animals.Select(WithLinks).ToList(),
            Links =
            [
                MakeLink("self",   nameof(GetAll), "GET"),
                MakeLink("create", nameof(Create), "POST"),
            ],
        });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalDto>> Get(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        return Ok(WithLinks(dto));
    }

    [HttpPost]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnimalDto>> Create(CreateAnimalRequest request)
    {
        var dto = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, WithLinks(dto));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalDto>> Update(Guid id, UpdateAnimalRequest request)
    {
        var dto = await _service.UpdateAsync(id, request);
        return Ok(WithLinks(dto));
    }

    [HttpGet("{id:guid}/movement")]
    [ProducesResponseType(typeof(CollectionResponse<MovementPointDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CollectionResponse<MovementPointDto>>> GetMovement(
        Guid id, [FromQuery] int limit = 50)
    {
        var points = await _service.GetMovementAsync(id, limit);
        return Ok(new CollectionResponse<MovementPointDto>
        {
            Items = points.Select(p => p with
            {
                Links =
                [
                    MakeLink("report", nameof(SightingReportController.Get), "SightingReport", "GET",
                        new { id = p.ReportId }),
                ],
            }).ToList(),
            Links =
            [
                MakeLink("self",   nameof(GetMovement), "GET", new { id, limit }),
                MakeLink("animal", nameof(Get),         "GET", new { id }),
            ],
        });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // ── HATEOAS ──────────────────────────────────────────────────────────────

    private AnimalDto WithLinks(AnimalDto dto) => dto with
    {
        Links =
        [
            MakeLink("self",     nameof(Get),         "GET",    new { id = dto.Id }),
            MakeLink("update",   nameof(Update),      "PUT",    new { id = dto.Id }),
            MakeLink("delete",   nameof(Delete),      "DELETE", new { id = dto.Id }),
            MakeLink("movement", nameof(GetMovement), "GET",    new { id = dto.Id }),
            MakeLink("reports",  nameof(SightingReportController.Search), "SightingReport", "GET",
                new { animalId = dto.Id }),
        ],
    };
}
