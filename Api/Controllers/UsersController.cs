using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Interfaces;
using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.API.Controllers;

[Route("api/users")]
[Authorize]
public class UsersController : ApiControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse<AppUserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<CollectionResponse<AppUserDto>>> GetAll()
    {
        var users = await _service.GetAllAsync();
        return Ok(new CollectionResponse<AppUserDto>
        {
            Items = users.Select(WithLinks).ToList(),
            Links =
            [
                MakeLink("self", nameof(GetAll), "GET"),
            ],
        });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppUserDto>> Get(Guid id)
    {
        var dto = await _service.GetByIdAsync(id);
        return Ok(WithLinks(dto));
    }

    [HttpPut("{id:guid}/role")]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppUserDto>> ChangeRole(Guid id, UpdateUserRoleRequest request)
    {
        var dto = await _service.ChangeRoleAsync(id, request.Role);
        return Ok(WithLinks(dto));
    }

    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppUserDto>> Activate(Guid id)
    {
        var dto = await _service.ActivateAsync(id);
        return Ok(WithLinks(dto));
    }

    [HttpPost("{id:guid}/deactivate")]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppUserDto>> Deactivate(Guid id)
    {
        var dto = await _service.DeactivateAsync(id);
        return Ok(WithLinks(dto));
    }

    // ── HATEOAS ──────────────────────────────────────────────────────────────

    private AppUserDto WithLinks(AppUserDto dto)
    {
        var links = new List<Link>
        {
            MakeLink("self",        nameof(Get),        "GET", new { id = dto.Id }),
            MakeLink("change-role", nameof(ChangeRole), "PUT", new { id = dto.Id }),
            MakeLink("reports",     nameof(SightingReportController.Search), "SightingReport", "GET",
                new { reportedByUserId = dto.Id }),
        };

        if (dto.IsActive)
            links.Add(MakeLink("deactivate", nameof(Deactivate), "POST", new { id = dto.Id }));
        else
            links.Add(MakeLink("activate", nameof(Activate), "POST", new { id = dto.Id }));

        return dto with { Links = links };
    }
}
