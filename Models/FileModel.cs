namespace SalvageCore.Models;

public class FileModel
{
    public Guid VehicleId { get; set; }
    public ICollection<IFormFile> Files { get; set; }
}