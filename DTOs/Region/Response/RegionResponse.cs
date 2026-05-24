namespace SalvageCore.DTOs.Region.Response;

public class RegionResponse
{
    public Guid RegionId { get; set; } = Guid.NewGuid();
    public string RegionIso { get; set; } = string.Empty;
    public string RegionName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}