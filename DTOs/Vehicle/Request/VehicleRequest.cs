using SalvageCore.Enums;

namespace SalvageCore.DTOs.Vehicle.Request;

public class VehicleRequest
{
    public Guid CompanyId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid MakeId { get; set; }
    public Guid ModelId { get; set; }
    public string? Year { get; set; } = string.Empty;
    public string? Mileage { get; set; } = string.Empty;
    public string? Engine { get; set; } = string.Empty;
    public string? Vin { get; set; } = string.Empty;
    public string? TitleStatus { get; set; } = string.Empty;
    public Guid? RegionId { get; set; }
    public DriveEnum Drive { get; set; }
    public TransmissionEnum Transmission { get; set; }
    public BodyStyle BodyStyle { get; set; }
    public string? ExteriorColor { get; set; } = string.Empty;
    public string? InteriorColor { get; set; } = string.Empty;
    public string? Highlights { get; set; } = string.Empty;
    public string? Issues { get; set; } = string.Empty;
    public string LastService { get; set; } = string.Empty;
    public string? SellerNotes { get; set; } = string.Empty;
    public ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();
}