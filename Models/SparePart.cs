using System.ComponentModel.DataAnnotations;

namespace SalvageCore.Models;

public class SparePart
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [MaxLength(100)] public string PartNumber { get; set; } = string.Empty;

    [MaxLength(100)] public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}