namespace SalvageCore.Models;

public class Questions
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}