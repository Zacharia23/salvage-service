namespace SalvageCore.Models;

public class UploadResponse
{
    public bool IsSuccess { get; set; } = false;
    public string ImageUrl { get; set; } = string.Empty;
}