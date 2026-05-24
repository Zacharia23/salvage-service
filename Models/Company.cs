using System.ComponentModel.DataAnnotations;
using SalvageCore.Enums;

namespace SalvageCore.Models;

public class Company
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Number { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;

    [Phone] public string Phone { get; set; } = string.Empty;

    [EmailAddress] public string Email { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;
    public StatusEnums Status { get; set; }
    public CompanyType CompanyType { get; set; }
    public string LogoUrl { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public ICollection<Vehicle>? Vehicles { get; set; } = new List<Vehicle>();
}