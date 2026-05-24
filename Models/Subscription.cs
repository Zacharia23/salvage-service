namespace SalvageCore.Models;

public class Subscription
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public Guid OfferId { get; set; }
    public virtual Offer Offer { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime? UnsubscribedTime { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}