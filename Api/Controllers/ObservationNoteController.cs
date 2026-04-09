using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

[ApiController]
[Route("api/notes")]
[Produces("application/json")]
public class ObservationNoteController : ControllerBase
{
    // TODO: replace with User.FindFirstValue(ClaimTypes.NameIdentifier) once JWT is implemented
    private static readonly Guid PlaceholderUserId = new("00000000-0000-0000-0000-000000000001");

    private readonly IObservationNoteService _service;

    public ObservationNoteController(IObservationNoteService service)
    {
        _service = service;
    }

    [HttpGet("report/{reportId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ObservationNoteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ObservationNoteDto>>> GetByReport(Guid reportId)
        => Ok(await _service.GetByReportIdAsync(reportId));

    [HttpPost]
    [ProducesResponseType(typeof(ObservationNoteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationNoteDto>> Create(CreateObservationNoteRequest request)
    {
        var result = await _service.CreateAsync(request, PlaceholderUserId);
        return CreatedAtAction(nameof(GetByReport), new { reportId = result.SightingReportId }, result);
    }
}
