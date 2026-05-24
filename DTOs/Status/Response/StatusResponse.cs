namespace SalvageCore.DTOs.Status.Response;

public class StatusResponse
{
    public Guid StatusId { get; set; }
    public int Value { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}