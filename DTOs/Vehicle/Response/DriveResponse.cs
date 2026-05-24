namespace SalvageCore.DTOs.Vehicle.Response;

public class DriveResponse
{
    public Guid DriveId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}