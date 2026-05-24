using Microsoft.EntityFrameworkCore;

namespace SalvageCore.DTOs.Offer.Response;

public class OfferDetailsResponse
{
    public Guid OfferId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string? OfferType { get; set; }

    [Precision(8, 12)] public double IncrementPrice { get; set; }

    [Precision(8, 12)] public double InitialPrice { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<BidsResponseList> Bids { get; set; } = new List<BidsResponseList>();
}