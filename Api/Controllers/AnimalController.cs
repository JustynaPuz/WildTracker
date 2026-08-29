using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

[Route("api/animals")]
[Authorize]
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
                MakeLink("search", nameof(Search), "GET"),
                MakeLink("create", nameof(Create), "POST"),
            ],
        });
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<AnimalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<AnimalDto>>> Search([FromQuery] AnimalSearchRequest request)
    {
        var result = await _service.SearchAsync(request);
        return Ok(result with
        {
            Items = result.Items.Select(WithLinks).ToList(),
            Links = PaginationLinks(result.Page, result.TotalPages, request),
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
    [Authorize(Roles = "Ranger,Admin")]
    [ProducesResponseType(typeof(AnimalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnimalDto>> Create(CreateAnimalRequest request)
    {
        var dto = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, WithLinks(dto));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Ranger,Admin")]
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
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

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

    private IReadOnlyList<Link> PaginationLinks(int page, int totalPages, AnimalSearchRequest req)
    {
        var links = new List<Link>
        {
            PageLink("self",  page,                    req),
            PageLink("first", 1,                       req),
            PageLink("last",  Math.Max(totalPages, 1), req),
        };
        if (page > 1)          links.Add(PageLink("prev", page - 1, req));
        if (page < totalPages) links.Add(PageLink("next", page + 1, req));
        return links;
    }

    private Link PageLink(string rel, int page, AnimalSearchRequest req) =>
        MakeLink(rel, nameof(Search), "GET", new
        {
            species      = req.Species,
            healthStatus = req.HealthStatus,
            searchTerm   = req.SearchTerm,
            page,
            pageSize     = req.PageSize,
        });
}
