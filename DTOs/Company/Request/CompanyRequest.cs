using System.ComponentModel.DataAnnotations;
using SalvageCore.Enums;

namespace SalvageCore.DTOs.Company.Request;

public class CompanyRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;

    [Phone] public string Phone { get; set; } = string.Empty;

    [EmailAddress] public string Email { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;
    public CompanyType CompanyType { get; set; }
    public IFormFile? File { get; set; }
}