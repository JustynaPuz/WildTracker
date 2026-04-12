using Microsoft.AspNetCore.Mvc;
using WildTracker.Contracts.Common;

namespace WildTracker.API.Controllers;

/// <summary>
/// Base controller that provides HATEOAS link-building helpers.
/// Url.ActionLink generates absolute URLs using the current request's scheme and host.
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>Build a link targeting an action on the current controller.</summary>
    protected Link MakeLink(string rel, string action, string method, object? values = null) =>
        new(rel, Url.ActionLink(action, values: values)!, method);

    /// <summary>Build a link targeting an action on a different controller.</summary>
    protected Link MakeLink(string rel, string action, string controller, string method, object? values = null) =>
        new(rel, Url.ActionLink(action, controller, values)!, method);
}
