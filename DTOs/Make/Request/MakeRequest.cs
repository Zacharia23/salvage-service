namespace SalvageCore.DTOs.Make.Request;

public class MakeRequest
{
    public string Name { get; set; } = string.Empty;
    public ICollection<ModelRequest> Models { get; set; } = new List<ModelRequest>();
}