using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WildTracker.Application.Exceptions;
using WildTracker.Contracts.Common;

namespace WildTracker.API.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid CurrentUserId =>
        Guid.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id)
            ? id
            : throw new UnauthorizedException("User not authenticated.");

    protected Link MakeLink(string rel, string action, string method, object? values = null) =>
        new(rel, Url.ActionLink(action, values: values)!, method);

    protected Link MakeLink(string rel, string action, string controller, string method, object? values = null) =>
        new(rel, Url.ActionLink(action, controller, values)!, method);
}
