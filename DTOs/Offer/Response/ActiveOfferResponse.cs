using SalvageCore.DTOs.Vehicle.Response;

namespace SalvageCore.DTOs.Offer.Response;

public class ActiveOfferResponse
{
    public Guid OfferId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Mileage { get; set; } = string.Empty;
    public bool Reserved { get; set; } = false;
    public string Vin { get; set; } = string.Empty;
    public string TitleStatus { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string RegionCode { get; set; } = string.Empty;
    public string Engine { get; set; } = string.Empty;
    public string DriveTrain { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public string BodyStyle { get; set; } = string.Empty;
    public string InteriorColor { get; set; } = string.Empty;
    public string ExteriorColor { get; set; } = string.Empty;
    public string SellerType { get; set; } = string.Empty;
    public string OfferType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double InitialPrice { get; set; }
    public double IncrementPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TotalBids { get; set; }
    public decimal LastOfferPrice { get; set; }
    public string Highlights { get; set; } = string.Empty;
    public string Equipments { get; set; } = string.Empty;
    public string Issues { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public ICollection<ImageResponse> Images { get; set; } = new List<ImageResponse>();
}