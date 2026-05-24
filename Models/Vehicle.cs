using SalvageCore.Enums;

namespace SalvageCore.Models;

public class Vehicle
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public string Reference { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Reserved { get; set; } = false;
    public Guid MakeId { get; set; }
    public virtual Make? Make { get; set; }
    public Guid ModelId { get; set; }
    public virtual Model? Model { get; set; }
    public string? Year { get; set; } = string.Empty;
    public string? Mileage { get; set; } = string.Empty;
    public string? Engine { get; set; } = string.Empty;
    public string? TitleStatus { get; set; } = string.Empty;
    public Guid? RegionId { get; set; }
    public virtual Region? Region { get; set; }
    public BodyStyle BodyStyle { get; set; }
    public DriveEnum Drive { get; set; }
    public TransmissionEnum Transmission { get; set; }
    public string? ExteriorColor { get; set; } = string.Empty;
    public string? InteriorColor { get; set; } = string.Empty;
    public string? Highlights { get; set; } = string.Empty;
    public string? Issues { get; set; } = string.Empty;
    public DateTime? LastService { get; set; }
    public string? SellerNotes { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public ICollection<VehicleImage>? VehicleImages { get; set; }
    public virtual Offer? Offer { get; set; } = null;
}