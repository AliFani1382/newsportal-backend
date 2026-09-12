using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Common;
using System.Net;

namespace NewsPortal.API.Extensions;

public static class ApiResponseExtensions
{
    public static IActionResult ToHttpResult<T>(this ApiResponse<T> response, Func<IActionResult>? onSuccess = null)
    {
        if (response.IsSuccess)
        {
            return onSuccess != null ? onSuccess() : new OkObjectResult(response);
        }

        return MapErrorToResult(response.ErrorCode, response);
    }

    public static IActionResult ToHttpResult(this ApiResponse response, Func<IActionResult>? onSuccess = null)
    {
        if (response.IsSuccess)
        {
            return onSuccess != null ? onSuccess() : new OkObjectResult(response);
        }

        return MapErrorToResult(response.ErrorCode, response);
    }

    private static IActionResult MapErrorToResult(ApiErrorCode errorCode, object response)
    {
        return errorCode switch
        {
            ApiErrorCode.NotFound => new NotFoundObjectResult(response),
            ApiErrorCode.BadRequest => new BadRequestObjectResult(response),
            ApiErrorCode.ValidationError => new BadRequestObjectResult(response),
            ApiErrorCode.Unauthorized => new UnauthorizedObjectResult(response),
            ApiErrorCode.Forbidden => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Forbidden },
            ApiErrorCode.Conflict => new ConflictObjectResult(response),
            _ => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError }
        };
    }
}
