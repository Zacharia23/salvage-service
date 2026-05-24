namespace SalvageCore.Models;

public class CustomerNotification
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public Guid NotificationId { get; set; }
    public virtual Notification Notification { get; set; } = null!;
    public bool IsRead { get; set; } = false;
    public DateTime? ReadTime { get; set; }
    public DateTime ReceivedTime { get; set; } = DateTime.UtcNow;
}