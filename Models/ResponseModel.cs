namespace SalvageCore.Models;

public class ResponseModel
{
    public int StatusCode { get; set; }
    public string? RequestUrl { get; set; }
    public string Title { get; set; }
    public object? Error { get; set; }
    public string? ContentType { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}