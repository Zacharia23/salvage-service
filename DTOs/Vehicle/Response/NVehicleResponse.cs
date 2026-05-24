namespace SalvageCore.DTOs.Vehicle.Response;

public class NVehicleResponse
{
    public Guid VehicleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public bool Reserved { get; set; } = false;
    public virtual MakeResponse? Make { get; set; }
}