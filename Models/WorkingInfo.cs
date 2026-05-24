namespace SalvageCore.Models;

public class WorkingInfo
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Title { get; set; } = string.Empty;
    public ICollection<Sections> Sections { get; set; } = new List<Sections>();
}