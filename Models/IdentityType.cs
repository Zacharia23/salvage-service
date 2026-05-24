namespace SalvageCore.Models;

public class IdentityType
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}