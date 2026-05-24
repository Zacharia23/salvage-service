using System.Net;

namespace SalvageCore.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException(string message, string errorCode, HttpStatusCode statusCode)
        : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    protected BaseException(string message, Exception innerException, string errorCode, HttpStatusCode statusCode)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    public string ErrorCode { get; }
    public HttpStatusCode StatusCode { get; }

    public class BusinessLogicException : BaseException
    {
        public BusinessLogicException(string message, string errorCode = "BUSINESS_ERROR")
            : base(message, errorCode, HttpStatusCode.BadRequest)
        {
        }

        public BusinessLogicException(string message, Exception innerException, string errorCode = "BUSINESS_ERROR")
            : base(message, innerException, errorCode, HttpStatusCode.BadRequest)
        {
        }
    }

    public class ValidationException : BaseException
    {
        public ValidationException(Dictionary<string, string[]> validationErrors)
            : base("One or more validation errors occurred.", "VALIDATION_ERROR", HttpStatusCode.BadRequest)
        {
            ValidationErrors = validationErrors;
        }

        public Dictionary<string, string[]> ValidationErrors { get; }
    }

    public class NotFoundException : BaseException
    {
        public NotFoundException(string message, string errorCode = "NOT_FOUND")
            : base(message, errorCode, HttpStatusCode.NotFound)
        {
        }
    }

    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message, string errorCode = "UNAUTHORIZED")
            : base(message, errorCode, HttpStatusCode.Unauthorized)
        {
        }
    }
}