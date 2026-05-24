using System.Diagnostics;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace SalvageCore.Infrastructure;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        Log.Error("Could not process request on {@Machine}. Trace ID {@TraceId}", Environment.MachineName, traceId);

        var (statusCode, title) = MapException(exception);
        var problemDetails = new ProblemDetails
        {
            Type = "",
            Status = statusCode,
            Title = title,
            Extensions = new Dictionary<string, object?>
            {
                { "traceId", traceId }
            }
        };
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Permission Denied"),
            UniqueConstraintException => (StatusCodes.Status500InternalServerError, exception.Message),
            ReferenceConstraintException => (StatusCodes.Status500InternalServerError, exception.Message),
            CannotInsertNullException => (StatusCodes.Status500InternalServerError, exception.Message),
            InvalidOperationException => (StatusCodes.Status500InternalServerError, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }
}