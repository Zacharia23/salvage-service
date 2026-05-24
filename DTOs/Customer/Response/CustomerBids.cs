using Microsoft.EntityFrameworkCore;

namespace SalvageCore.DTOs.Customer.Response;

public class CustomerBids
{
    public Guid BidId { get; set; }
    public Guid OfferId { get; set; }
    public string BidReference { get; set; } = string.Empty;
    public string OfferReference { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public Guid SystemUserId { get; set; }
    public decimal PreviousAmount { get; set; }

    [Precision(18, 2)] public decimal SubmittedAmount { get; set; }

    public bool Awarded { get; set; }
    public DateTime CreatedDate { get; set; }
}