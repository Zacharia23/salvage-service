using SalvageCore.Enums;

namespace SalvageCore.Models;

public class Notification
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; } = NotificationType.Info;
    public NotificationTarget Target { get; set; } = NotificationTarget.Specific;
    public Guid? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; } = null;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Scheduled { get; set; }
    public bool IsSent { get; set; } = false;
    public DateTime? Delivered { get; set; }
    public virtual ICollection<CustomerNotification> CustomerNotifications { get; set; } = new List<CustomerNotification>();
}