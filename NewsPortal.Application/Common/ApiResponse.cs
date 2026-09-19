using System.Text.Json.Serialization;

namespace NewsPortal.Application.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    public ApiErrorCode ErrorCode { get; set; } = ApiErrorCode.None;

    public static ApiResponse<T> Success(T data, string message = "عملیات با موفقیت انجام شد.")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            ErrorCode = ApiErrorCode.None
        };
    }

    public static ApiResponse<T> Failure(ApiErrorCode errorCode, string error, string message = "عملیات با خطا مواجه شد.")
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = new List<string> { error }
        };
    }

    public static ApiResponse<T> Failure(ApiErrorCode errorCode, List<string> errors, string message = "عملیات با خطا مواجه شد.")
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = errors
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Success(string message = "عملیات با موفقیت انجام شد.")
    {
        return new ApiResponse
        {
            IsSuccess = true,
            Message = message,
            Data = null,
            ErrorCode = ApiErrorCode.None
        };
    }

    public static new ApiResponse Failure(ApiErrorCode errorCode, string error, string message = "عملیات با خطا مواجه شد.")
    {
        return new ApiResponse
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = new List<string> { error },
            Data = null
        };
    }

    public static new ApiResponse Failure(ApiErrorCode errorCode, List<string> errors, string message = "عملیات با خطا مواجه شد.")
    {
        return new ApiResponse
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = errors,
            Data = null
        };
    }
}
