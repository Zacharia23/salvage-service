using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SalvageCore.Enums;

namespace SalvageCore.Models;

public class Offer
{
    public Eligibility Eligibility = Eligibility.All;
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public NatureEnum OfferNature { get; set; }
    public EntityTypeEnum EntityType { get; set; }

    [MaxLength(100)] public string ReferenceNumber { get; set; } = string.Empty;

    public Guid? VehicleId { get; set; }
    public virtual Vehicle? Vehicle { get; set; } = null;
    public Guid? SparePartId { get; set; }
    public virtual SparePart? SparePart { get; set; } = null;

    [Precision(8, 12)] public double IncrementPrice { get; set; }

    [Precision(8, 12)] public double ReservePrice { get; set; }

    public int Views { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Extended { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public StatusEnums Status { get; set; } = StatusEnums.Active;
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
}