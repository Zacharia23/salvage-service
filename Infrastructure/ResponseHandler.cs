using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SalvageCore.Models;

namespace SalvageCore.Infrastructure;

public class ResponseHandler
{
    public static ResponseModel HandleSuccess(string message, object result, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;

        return new ResponseModel
        {
            RequestUrl = context.Request.GetDisplayUrl(),
            Title = "SUCCESS",
            StatusCode = context.Response.StatusCode,
            ContentType = context.Response.ContentType,
            Message = message,
            Data = result,
            Error = null
        };
    }

    // Wrapper for error responses
    public static ResponseModel HandleError(HttpContext context, int statusCode, string errorMessage)
    {
        context.Response.StatusCode = statusCode;

        return new ResponseModel
        {
            RequestUrl = context.Request.GetDisplayUrl(),
            Title = "FAILED",
            StatusCode = statusCode,
            ContentType = context.Response.ContentType,
            Message = errorMessage,
            Data = null,
            Error = errorMessage
        };
    }

    // Specifically handle ModelState errors (Bad Request)
    public static ResponseModel HandleValidationError(HttpContext context, ModelStateDictionary modelState)
    {
        var validationErrors = new Dictionary<string, string[]>();

        foreach (var keyModelStatePair in modelState)
        {
            var key = keyModelStatePair.Key;
            var errors = keyModelStatePair.Value.Errors;

            if (errors != null && errors.Count > 0)
            {
                var errorMessages = errors.Select(error => error.ErrorMessage).ToArray();
                validationErrors.Add(key, errorMessages);
            }
        }

        var problemDetails = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            StatusCode = StatusCodes.Status400BadRequest,
            TraceId = context.TraceIdentifier,
            Error = validationErrors
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return new ResponseModel
        {
            RequestUrl = context.Request.GetDisplayUrl(),
            Title = "FAILED",
            StatusCode = StatusCodes.Status400BadRequest,
            ContentType = context.Response.ContentType,
            Message = "One or more errors occurred while processing your request.",
            Data = null,
            Error = problemDetails
        };
    }

    public static ResponseModel ResponseWrapper(string message, object? result, HttpContext context, object? exception = null)
    {
        var response = new ResponseModel
        {
            RequestUrl = context.Request.GetDisplayUrl(),
            Title = "SUCCESS",
            StatusCode = context.Response.StatusCode,
            ContentType = context.Response.ContentType,
            Message = message,
            Data = result,
            Error = exception
        };

        return response;
    }
}