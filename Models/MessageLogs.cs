using System.ComponentModel.DataAnnotations;

namespace SalvageCore.Models;

public class MessageLogs
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [MaxLength(100)] public string Phone { get; set; } = string.Empty;

    [MaxLength(100)] public string RequestId { get; set; } = string.Empty;

    [MaxLength(100)] public string DeliveryStatus { get; set; } = string.Empty;

    [MaxLength(500)] public string Content { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}