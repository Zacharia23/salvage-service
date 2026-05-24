namespace SalvageCore.DTOs.Offer.Response;

public class BidsResponseList
{
    public Guid BidId { get; set; }
    public Guid OfferId { get; set; }
    public string BidReference { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string OfferReference { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public decimal PreviousAmount { get; set; }
    public decimal SubmittedAmount { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public bool Awarded { get; set; } = false;
    public DateTime CreatedDate { get; set; }
}