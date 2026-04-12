using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

/// <summary>
/// Notes are sub-resources of reports: /api/reports/{reportId}/notes
/// </summary>
[Route("api/reports/{reportId:guid}/notes")]
public class ObservationNoteController : ApiControllerBase
{
    // TODO: replace with User.FindFirstValue(ClaimTypes.NameIdentifier) once JWT is implemented
    private static readonly Guid PlaceholderUserId = new("00000000-0000-0000-0000-000000000001");

    private readonly IObservationNoteService _service;

    public ObservationNoteController(IObservationNoteService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse<ObservationNoteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<CollectionResponse<ObservationNoteDto>>> GetByReport(Guid reportId)
    {
        var notes = await _service.GetByReportIdAsync(reportId);
        return Ok(new CollectionResponse<ObservationNoteDto>
        {
            Items = notes.Select(n => WithLinks(n, reportId)).ToList(),
            Links =
            [
                MakeLink("self",    nameof(GetByReport), "ObservationNote", "GET",  new { reportId }),
                MakeLink("add",     nameof(Create),      "ObservationNote", "POST", new { reportId }),
                MakeLink("report",  nameof(SightingReportController.Get), "SightingReport", "GET",
                    new { id = reportId }),
            ],
        });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ObservationNoteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationNoteDto>> Create(Guid reportId, CreateObservationNoteRequest request)
    {
        var dto = await _service.CreateAsync(reportId, request.Content, PlaceholderUserId);
        return CreatedAtAction(nameof(GetByReport), new { reportId }, WithLinks(dto, reportId));
    }

    // ── HATEOAS ──────────────────────────────────────────────────────────────

    private ObservationNoteDto WithLinks(ObservationNoteDto dto, Guid reportId) => dto with
    {
        Links =
        [
            MakeLink("notes",  nameof(GetByReport), "ObservationNote", "GET", new { reportId }),
            MakeLink("report", nameof(SightingReportController.Get), "SightingReport", "GET",
                new { id = reportId }),
        ],
    };
}
