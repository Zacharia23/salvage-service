namespace SalvageCore.Models;

public class Region
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string RegionIso { get; set; } = string.Empty;
    public string RegionName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Vehicle>? Vehicles { get; set; }
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}