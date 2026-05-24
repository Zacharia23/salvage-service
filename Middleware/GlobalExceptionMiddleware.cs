using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalvageCore.Exceptions;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly IWebHostEnvironment _environment;
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            TraceId = context.TraceIdentifier
        };

        // Handle different exception types
        switch (exception)
        {
            case BaseException baseEx:
                response.StatusCode = (int)baseEx.StatusCode;
                errorResponse.Message = baseEx.Message;
                errorResponse.ErrorCode = baseEx.ErrorCode;

                // Special handling for validation exceptions
                if (baseEx is BaseException.ValidationException validationEx) errorResponse.ValidationErrors = validationEx.ValidationErrors;

                Log.Warning("Application exception occurred: {@Exception}", new
                {
                    baseEx.Message,
                    baseEx.ErrorCode,
                    baseEx.StatusCode,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path,
                    context.Request.Method
                });
                break;

            case DbUpdateException dbEx when dbEx.InnerException is SqlException sqlEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.ErrorCode = "DATABASE_ERROR";

                errorResponse.Message = sqlEx.Number switch
                {
                    2627 or 2601 => "A record with this information already exists.",
                    547 => "Cannot delete this record because it's being used by other records.",
                    2 => "Database connection timeout. Please try again.",
                    _ => "A database error occurred. Please try again."
                };

                Log.Error("Database exception occurred: {@Exception}", new
                {
                    SqlErrorNumber = sqlEx.Number,
                    sqlEx.Message,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path,
                    context.Request.Method,
                    StackTrace = _environment.IsDevelopment() ? exception.StackTrace : null
                });
                break;

            case DbUpdateConcurrencyException:
                response.StatusCode = (int)HttpStatusCode.Conflict;
                errorResponse.Message = "The record was modified by another user. Please refresh and try again.";
                errorResponse.ErrorCode = "CONCURRENCY_ERROR";

                Log.Warning("Concurrency exception occurred: {@Exception}", new
                {
                    exception.Message,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path
                });
                break;

            case TimeoutException:
                response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                errorResponse.Message = "The operation timed out. Please try again.";
                errorResponse.ErrorCode = "TIMEOUT_ERROR";

                Log.Warning("Timeout exception occurred: {@Exception}", new
                {
                    exception.Message,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path
                });
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = "You don't have permission to perform this action.";
                errorResponse.ErrorCode = "UNAUTHORIZED";

                Log.Warning("Unauthorized access attempt: {@Exception}", new
                {
                    exception.Message,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path,
                    User = context.User?.Identity?.Name
                });
                break;

            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = argEx.Message;
                errorResponse.ErrorCode = "INVALID_ARGUMENT";

                Log.Warning("Invalid argument exception: {@Exception}", new
                {
                    argEx.Message,
                    Parameter = argEx.ParamName,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path
                });
                break;

            case InvalidOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "The requested operation is not valid in the current state.";
                errorResponse.ErrorCode = "INVALID_OPERATION";

                Log.Error("Invalid operation exception: {@Exception}", new
                {
                    exception.Message,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path,
                    StackTrace = _environment.IsDevelopment() ? exception.StackTrace : null
                });
                break;

            default:
                // Unhandled exceptions
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An unexpected error occurred. Please try again later.";
                errorResponse.ErrorCode = "INTERNAL_SERVER_ERROR";

                Log.Error("Unhandled exception occurred: {@Exception}", new
                {
                    Type = exception.GetType().Name,
                    exception.Message,
                    TraceId = context.TraceIdentifier,
                    context.Request.Path,
                    context.Request.Method,
                    exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                });
                break;
        }

        // Add detailed error information in development
        if (_environment.IsDevelopment()) errorResponse.Details = exception.ToString();

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(jsonResponse);
    }
}