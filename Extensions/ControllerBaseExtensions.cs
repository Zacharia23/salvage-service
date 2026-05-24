using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SalvageCore.Infrastructure;

namespace SalvageCore.Extensions;

public static class ControllerBaseExtensions
{
    public static ActionResult Respond(this ControllerBase controller, string message, object result)
    {
        var response = ResponseHandler.HandleSuccess(message, result, controller.HttpContext);
        return controller.Ok(response);
    }

    public static ActionResult RespondBadRequest(this ControllerBase controller, ModelStateDictionary modelState)
    {
        var response = ResponseHandler.HandleValidationError(controller.HttpContext, modelState);
        return controller.BadRequest(response);
    }

    public static ActionResult RespondError(this ControllerBase controller, int statusCode, string errorMessage)
    {
        var response = ResponseHandler.HandleError(controller.HttpContext, statusCode, errorMessage);
        return controller.StatusCode(statusCode, response);
    }

    public static ActionResult RespondNotFound(this ControllerBase controller, string errorMessage = "Resource entity not found")
    {
        return controller.RespondError(StatusCodes.Status404NotFound, errorMessage);
    }

    public static ActionResult RespondServerError(this ControllerBase controller, string errorMessage = "Internal server error occured")
    {
        return controller.RespondError(StatusCodes.Status500InternalServerError, errorMessage);
    }
}