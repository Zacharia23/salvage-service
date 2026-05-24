using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SalvageCore.Models;

public class Bid
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid OfferId { get; set; }
    public virtual Offer? Offer { get; set; }
    [MaxLength(100)] public string BidReference { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    [Precision(18, 2)] public decimal PreviousAmount { get; set; }

    [Precision(18, 2)] public decimal SubmittedAmount { get; set; }

    public bool Awarded { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}