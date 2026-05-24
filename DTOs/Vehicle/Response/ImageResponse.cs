namespace SalvageCore.DTOs.Vehicle.Response;

public class ImageResponse
{
    public Guid VehicleImageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}