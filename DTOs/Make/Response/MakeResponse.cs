namespace SalvageCore.DTOs.Make.Response;

public class MakeResponse
{
    public Guid MakeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ModelResponse>? Models { get; set; }
}