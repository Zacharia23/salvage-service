using System.ComponentModel.DataAnnotations;

namespace SalvageCore.Models;

public class Invoice
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [MaxLength(100)] public string Reference { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public Guid BidId { get; set; }
    public virtual Bid Bid { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}