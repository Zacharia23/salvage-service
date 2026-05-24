namespace SalvageCore.DTOs.Vehicle.Response;

public class ModelResponse
{
    public Guid ModelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}