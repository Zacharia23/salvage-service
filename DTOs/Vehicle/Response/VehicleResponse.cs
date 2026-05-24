using SalvageCore.DTOs.Company.Response;
using SalvageCore.DTOs.Region.Response;
using SalvageCore.DTOs.Status.Response;

namespace SalvageCore.DTOs.Vehicle.Response;

public class VehicleResponse
{
    public Guid VehicleId { get; set; }
    public Guid CompanyId { get; set; }
    public CompanyResponse? Company { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public bool Reserved { get; set; } = false;
    public virtual MakeResponse? Make { get; set; }
    public virtual ModelResponse? Model { get; set; }
    public string? Year { get; set; } = string.Empty;
    public string? Mileage { get; set; } = string.Empty;
    public string? Engine { get; set; } = string.Empty;
    public string? Vin { get; set; } = string.Empty;
    public string? TitleStatus { get; set; } = string.Empty;
    public virtual RegionResponse? Region { get; set; }
    public virtual DriveResponse? Drive { get; set; }
    public virtual TransmissionResponse? Transmission { get; set; }
    public virtual BodyStyleResponse? BodyStyle { get; set; }
    public string? ExteriorColor { get; set; } = string.Empty;
    public string? InteriorColor { get; set; } = string.Empty;
    public string? Highlights { get; set; } = string.Empty;
    public string? Equipments { get; set; } = string.Empty;
    public string? Modifications { get; set; } = string.Empty;
    public string? Issues { get; set; } = string.Empty;
    public DateTime? LastService { get; set; }
    public string? SellerNotes { get; set; } = string.Empty;
    public StatusResponse? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<ImageResponse>? VehicleImages { get; set; }
}