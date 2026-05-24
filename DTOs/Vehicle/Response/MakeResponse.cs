namespace SalvageCore.DTOs.Vehicle.Response;

public class MakeResponse
{
    public Guid MakeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}