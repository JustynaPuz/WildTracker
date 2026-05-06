using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;

namespace WildTracker.API.Controllers;

[Route("api/stats")]
public class StatsController : ApiControllerBase
{
    private readonly IStatsService _service;

    public StatsController(IStatsService service) => _service = service;

    [HttpGet("summary")]
    [ProducesResponseType(typeof(StatsSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<StatsSummaryDto>> GetSummary()
    {
        var dto = await _service.GetSummaryAsync();
        return Ok(dto with
        {
            Links =
            [
                MakeLink("self",       nameof(GetSummary),   "GET"),
                MakeLink("by-species", nameof(GetBySpecies), "GET"),
                MakeLink("by-month",   nameof(GetByMonth),   "GET"),
            ],
        });
    }

    [HttpGet("by-species")]
    [ProducesResponseType(typeof(CollectionResponse<SightingsBySpeciesDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<CollectionResponse<SightingsBySpeciesDto>>> GetBySpecies()
    {
        var items = await _service.GetBySpeciesAsync();
        return Ok(new CollectionResponse<SightingsBySpeciesDto>
        {
            Items = items,
            Links =
            [
                MakeLink("self",    nameof(GetBySpecies), "GET"),
                MakeLink("summary", nameof(GetSummary),   "GET"),
            ],
        });
    }

    [HttpGet("by-month")]
    [ProducesResponseType(typeof(CollectionResponse<SightingsByMonthDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<CollectionResponse<SightingsByMonthDto>>> GetByMonth(
        [FromQuery] int months = 12)
    {
        var items = await _service.GetByMonthAsync(months);
        return Ok(new CollectionResponse<SightingsByMonthDto>
        {
            Items = items,
            Links =
            [
                MakeLink("self",    nameof(GetByMonth),   "GET"),
                MakeLink("summary", nameof(GetSummary),   "GET"),
            ],
        });
    }
}
