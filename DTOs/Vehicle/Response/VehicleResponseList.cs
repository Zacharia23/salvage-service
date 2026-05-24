namespace SalvageCore.DTOs.Vehicle.Response;

public class VehicleResponseList
{
    public Guid VehicleId { get; set; }
    public Guid CompanyId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; } = string.Empty;
    public bool Reserved { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string? Year { get; set; } = string.Empty;
    public string? Mileage { get; set; } = string.Empty;
    public string? Engine { get; set; } = string.Empty;
    public string? TitleStatus { get; set; } = string.Empty;
    public string? Region { get; set; } = string.Empty;
    public string? Drive { get; set; } = string.Empty;
    public string? Transmission { get; set; } = string.Empty;
    public string? BodyStyle { get; set; } = string.Empty;
    public string? ExteriorColor { get; set; } = string.Empty;
    public string? InteriorColor { get; set; } = string.Empty;
    public string? Highlights { get; set; } = string.Empty;
    public string? Issues { get; set; } = string.Empty;
    public DateTime? LastService { get; set; }
    public string? SellerNotes { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}