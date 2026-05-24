using System.ComponentModel.DataAnnotations;

namespace SalvageCore.Models;

public class Model
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [MaxLength(200)] public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid MakeId { get; set; }
    public virtual Make Make { get; set; } = null!;
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}