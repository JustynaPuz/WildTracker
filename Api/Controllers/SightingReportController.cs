using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

[ApiController]
[Route("api/reports")]
[Produces("application/json")]
public class SightingReportController : ControllerBase
{
    // TODO: replace with User.FindFirstValue(ClaimTypes.NameIdentifier) once JWT is implemented
    private static readonly Guid PlaceholderUserId = new("00000000-0000-0000-0000-000000000001");

    private readonly ISightingReportService _service;

    public SightingReportController(ISightingReportService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SightingReportDto>> Get(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SightingReportDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SightingReportDto>>> Search([FromQuery] SightingReportSearchRequest request)
        => Ok(await _service.SearchAsync(request));

    [HttpPost]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SightingReportDto>> Create(CreateSightingReportRequest request)
    {
        var result = await _service.CreateAsync(request, PlaceholderUserId);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SightingReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SightingReportDto>> Update(Guid id, UpdateSightingReportRequest request)
        => Ok(await _service.UpdateAsync(id, request));

    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _service.ApproveAsync(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _service.RejectAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
