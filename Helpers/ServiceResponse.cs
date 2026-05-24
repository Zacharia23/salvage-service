namespace SalvageCore.Helpers;

public class ServiceResponse<T>
{
    public ServiceResponse()
    {
        Errors = new List<string>();
    }

    public ServiceResponse(T data)
    {
        Success = true;
        Data = data;
        Errors = new List<string>();
    }

    public ServiceResponse(string message)
    {
        Success = false;
        Message = message;
        Errors = new List<string>();
    }

    public ServiceResponse(List<string> errors)
    {
        Success = false;
        Errors = errors ?? new List<string>();
    }

    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; }
}

public static class ServiceResponse
{
    public static ServiceResponse<T> Success<T>(T data, string? message = null)
    {
        return new ServiceResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Errors = new List<string>()
        };
    }

    public static ServiceResponse<T> Failure<T>(string message)
    {
        return new ServiceResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Errors = new List<string>()
        };
    }

    public static ServiceResponse<T> Failure<T>(List<string> errors)
    {
        return new ServiceResponse<T>
        {
            Success = false,
            Data = default,
            Errors = errors ?? new List<string>()
        };
    }

    public static ServiceResponse<T> Failure<T>(string message, List<string> errors)
    {
        return new ServiceResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}