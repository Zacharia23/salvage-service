namespace SalvageCore.DTOs.Vehicle.Response;

public class VehicleMiniResponseList
{
    public Guid VehicleId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}