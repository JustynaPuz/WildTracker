using Microsoft.AspNetCore.Mvc;
using WildTracker.Contracts.Common;

namespace WildTracker.API.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    protected Link MakeLink(string rel, string action, string method, object? values = null) =>
        new(rel, Url.ActionLink(action, values: values)!, method);

    protected Link MakeLink(string rel, string action, string controller, string method, object? values = null) =>
        new(rel, Url.ActionLink(action, controller, values)!, method);
}
