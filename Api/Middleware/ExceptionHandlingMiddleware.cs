using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WildTracker.Application.Exceptions;

namespace WildTracker.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next   = next;
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
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status409Conflict, ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (ValidationException ex)
        {
            await WriteValidationProblemAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, int status, string detail)
    {
        var problem = new ProblemDetails
        {
            Type   = $"https://tools.ietf.org/html/rfc9110#section-15.5.{status - 399}",
            Title  = ReasonPhrases.GetReasonPhrase(status),
            Status = status,
            Detail = detail,
        };

        context.Response.StatusCode  = status;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsJsonAsync(problem);
    }

    private static Task WriteValidationProblemAsync(HttpContext context, ValidationException ex)
    {
        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Type   = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title  = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
        };

        context.Response.StatusCode  = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsJsonAsync(problem);
    }
}
