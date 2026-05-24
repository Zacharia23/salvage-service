namespace SalvageCore.Models;

public class ValidationProblemDetails
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int StatusCode { get; set; }
    public string TraceId { get; set; }
    public Dictionary<string, string[]> Error { get; set; }
}