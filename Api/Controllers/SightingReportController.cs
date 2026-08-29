using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Enums;

namespace WildTracker.API.Controllers;

[Route("api/reports")]
[Authorize]
public class SightingReportController : ApiControllerBase
{
    private readonly ISightingReportService _service;

    public SightingReportController(ISightingReportService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SightingReportDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SightingReportDto>>> Search([FromQuery] SightingReportSearchRequest request)
    {
        var result = await _service.SearchAsync(request);
        return Ok(result with
        {
            Items = result.Items.Select(WithLinks).ToList(),
            Links = PaginationLinks(result.Page, result.TotalPages, request),
        });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SightingReportDto>> Get(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        return Ok(WithLinks(dto));
    }

    [HttpPost]
    [Authorize(Roles = "Ranger,Admin")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SightingReportDto>> Create(CreateSightingReportRequest request)
    {
        var dto = await _service.CreateAsync(request, CurrentUserId);
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, WithLinks(dto));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Ranger,Admin")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SightingReportDto>> Update(Guid id, UpdateSightingReportRequest request)
    {
        var dto = await _service.UpdateAsync(id, request);
        return Ok(WithLinks(dto));
    }

    [HttpPost("{id:guid}/approve")]
    [Authorize(Roles = "Ranger,Admin")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SightingReportDto>> Approve(Guid id)
    {
        var dto = await _service.ApproveAsync(id);
        return Ok(WithLinks(dto));
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize(Roles = "Ranger,Admin")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SightingReportDto>> Reject(Guid id)
    {
        var dto = await _service.RejectAsync(id);
        return Ok(WithLinks(dto));
    }

    [HttpPost("{id:guid}/resolve")]
    [Authorize(Roles = "Ranger,Admin")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SightingReportDto>> Resolve(Guid id)
    {
        var dto = await _service.ResolveAsync(id);
        return Ok(WithLinks(dto));
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

    private SightingReportDto WithLinks(SightingReportDto dto)
    {
        var links = new List<Link>
        {
            MakeLink("self",   nameof(Get),    "GET",    new { id = dto.Id }),
            MakeLink("update", nameof(Update), "PUT",    new { id = dto.Id }),
            MakeLink("delete", nameof(Delete), "DELETE", new { id = dto.Id }),
            MakeLink("animal", nameof(AnimalController.Get), "Animal", "GET", new { id = dto.AnimalId }),
            MakeLink("notes",  nameof(ObservationNoteController.GetByReport), "ObservationNote", "GET",
                new { reportId = dto.Id }),
        };

        if (dto.Status == ReportStatus.Pending)
        {
            links.Add(MakeLink("approve", nameof(Approve), "POST", new { id = dto.Id }));
            links.Add(MakeLink("reject",  nameof(Reject),  "POST", new { id = dto.Id }));
        }

        if (dto.Status == ReportStatus.Verified)
            links.Add(MakeLink("resolve", nameof(Resolve), "POST", new { id = dto.Id }));

        return dto with { Links = links };
    }

    private IReadOnlyList<Link> PaginationLinks(int page, int totalPages, SightingReportSearchRequest req)
    {
        var links = new List<Link>
        {
            PageLink("self",  page,       req),
            PageLink("first", 1,          req),
            PageLink("last",  Math.Max(totalPages, 1), req),
        };
        if (page > 1)          links.Add(PageLink("prev", page - 1, req));
        if (page < totalPages) links.Add(PageLink("next", page + 1, req));
        return links;
    }

    private Link PageLink(string rel, int page, SightingReportSearchRequest req) =>
        MakeLink(rel, nameof(Search), "GET", new
        {
            animalId         = req.AnimalId,
            animalSearchTerm = req.AnimalSearchTerm,
            species          = req.Species,
            reportedByUserId = req.ReportedByUserId,
            status           = req.Status,
            from             = req.From,
            to               = req.To,
            page,
            pageSize         = req.PageSize,
        });
}
