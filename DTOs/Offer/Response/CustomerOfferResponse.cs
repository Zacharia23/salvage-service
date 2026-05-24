using SalvageCore.DTOs.Vehicle.Response;

namespace SalvageCore.DTOs.Offer.Response;

public class CustomerOfferResponse
{
    public Guid OfferId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string RegionCode { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public bool Reserved { get; set; }
    public string Model { get; set; } = string.Empty;
    public string OfferType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double InitialPrice { get; set; }
    public double IncrementPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<ImageResponse> Images { get; set; } = new List<ImageResponse>();
}