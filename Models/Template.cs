using System.ComponentModel.DataAnnotations;
using SalvageCore.Enums;

namespace SalvageCore.Models;

public class Template
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(100)] public string Name { get; set; } = string.Empty;

    public NotificationChannels Channel { get; set; }

    [MaxLength(1000)] public string Content { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}