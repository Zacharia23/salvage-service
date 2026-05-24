namespace SalvageCore.Models;

public class VehicleImage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string ImageUrl { get; set; } = string.Empty;
    public Guid VehicleId { get; set; }
    public virtual Vehicle? Vehicle { get; set; }
}