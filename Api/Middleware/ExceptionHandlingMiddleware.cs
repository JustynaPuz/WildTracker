using System.Net;
using System.Text.Json;
using FluentValidation;
using WildTracker.Application.Exceptions;

namespace WildTracker.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (ValidationException ex)
        {
            await WriteValidationProblemAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode status, string detail)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)status;

        var problem = new
        {
            type = $"https://httpstatuses.com/{(int)status}",
            title = status.ToString(),
            status = (int)status,
            detail
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }

    private static async Task WriteValidationProblemAsync(HttpContext context, ValidationException ex)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problem = new
        {
            type = "https://httpstatuses.com/400",
            title = "Validation failed",
            status = 400,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
