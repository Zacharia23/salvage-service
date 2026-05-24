namespace SalvageCore.Models;

public class Sections
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Content { get; set; } = string.Empty;
}