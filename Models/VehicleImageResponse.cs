namespace SalvageCore.Models;

public class VehicleImageResponse
{
    public bool IsSuccess { get; set; } = false;
    public ICollection<string> ImageUrl { get; set; } = new List<string>();
}