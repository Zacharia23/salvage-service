using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.Company.Response;

public class CompanyResponse
{
    public Guid CompanyId { get; set; }
    public string? CompanyType { get; set; }
    public string Number { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;

    [Phone] public string Phone { get; set; } = string.Empty;

    [EmailAddress] public string Email { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? CompanyLogo { get; set; }
    public int TotalVehicles { get; set; }
    public string? Status { get; set; }
}