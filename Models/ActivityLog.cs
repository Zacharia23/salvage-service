using SalvageCore.Enums;

namespace SalvageCore.Models;

public class ActivityLog
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public EventType EventType { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Agent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}