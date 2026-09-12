namespace NewsPortal.Application.Common;

public enum ApiErrorCode
{
    None = 0,
    NotFound = 1,
    ValidationError = 2,
    Unauthorized = 3,
    Forbidden = 4,
    Conflict = 5,
    BadRequest = 6,
    InternalError = 7
}
