using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Common;
using System.Net;

namespace NewsPortal.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "یک خطای پیش‌بینی‌نشده در سیستم رخ داد: {Message}", exception.Message);

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var response = ApiResponse.Failure(
            ApiErrorCode.InternalError,
            "خطای داخلی سرور رخ داده است. لطفاً بعداً دوباره تلاش کنید.");

     

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}

