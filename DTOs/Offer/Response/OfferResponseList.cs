namespace SalvageCore.DTOs.Offer.Response;

public class OfferResponseList
{
    public Guid OfferId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string OfferType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double InitialPrice { get; set; }
    public double IncrementPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TotalBids { get; set; }
}