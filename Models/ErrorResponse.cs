namespace SalvageCore.Models;

public class ErrorResponse
{
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public string TraceId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Dictionary<string, string[]> ValidationErrors { get; set; }
    public string Details { get; set; }
}