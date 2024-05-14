using System.ComponentModel.DataAnnotations;
using System.Net;

namespace OrderAggregator.Middlewares;

/// <summary>
/// Middleware that respond with appropriate status code when error occurs during request processing.
/// <remarks>This could be further enhanced using ProblemDetails.</remarks>
/// </summary>
/// <param name="next">Function processing the request throughout middleware pipeline.</param>
/// <param name="logger">Logger for errors and diagnostics.</param>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException ex) when (!context.RequestAborted.IsCancellationRequested)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unexpected error occurred.");

        context.Response.StatusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.ContentType = "text/plain";

        await context.Response.WriteAsync(exception.Message);
    }
}
