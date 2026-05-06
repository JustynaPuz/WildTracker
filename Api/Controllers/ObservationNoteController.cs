using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

[Route("api/reports/{reportId:guid}/notes")]
[Authorize]
public class ObservationNoteController : ApiControllerBase
{
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
                MakeLink("self",   nameof(GetByReport), "ObservationNote", "GET",  new { reportId }),
                MakeLink("create", nameof(Create),      "ObservationNote", "POST", new { reportId }),
                MakeLink("report", nameof(SightingReportController.Get), "SightingReport", "GET",
                    new { id = reportId }),
            ],
        });
    }

    [HttpGet("{noteId:guid}")]
    [ProducesResponseType(typeof(ObservationNoteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationNoteDto>> GetById(Guid reportId, Guid noteId)
    {
        var notes = await _service.GetByReportIdAsync(reportId);
        var note  = notes.FirstOrDefault(n => n.Id == noteId);
        if (note is null) return NotFound();
        return Ok(WithLinks(note, reportId));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ObservationNoteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationNoteDto>> Create(Guid reportId, CreateObservationNoteRequest request)
    {
        var dto = await _service.CreateAsync(reportId, request.Content, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { reportId, noteId = dto.Id }, WithLinks(dto, reportId));
    }

    [HttpPut("{noteId:guid}")]
    [ProducesResponseType(typeof(ObservationNoteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationNoteDto>> Update(
        Guid reportId, Guid noteId, UpdateObservationNoteRequest request)
    {
        var dto = await _service.UpdateAsync(reportId, noteId, request.Content);
        return Ok(WithLinks(dto, reportId));
    }

    [HttpDelete("{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid reportId, Guid noteId)
    {
        await _service.DeleteAsync(reportId, noteId);
        return NoContent();
    }

    private ObservationNoteDto WithLinks(ObservationNoteDto dto, Guid reportId) => dto with
    {
        Links =
        [
            MakeLink("self",   nameof(GetById),    "ObservationNote", "GET",    new { reportId, noteId = dto.Id }),
            MakeLink("update", nameof(Update),     "ObservationNote", "PUT",    new { reportId, noteId = dto.Id }),
            MakeLink("delete", nameof(Delete),     "ObservationNote", "DELETE", new { reportId, noteId = dto.Id }),
            MakeLink("notes",  nameof(GetByReport),"ObservationNote", "GET",    new { reportId }),
            MakeLink("report", nameof(SightingReportController.Get), "SightingReport", "GET",
                new { id = reportId }),
        ],
    };
}
