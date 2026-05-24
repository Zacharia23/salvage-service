using System.ComponentModel.DataAnnotations;
using SalvageCore.DTOs.Invoices.Response;
using SalvageCore.DTOs.Offer.Response;
using SalvageCore.DTOs.Vehicle.Response;

namespace SalvageCore.DTOs.Company.Response;

public class CompanyDetailsResponse
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
    public string? Status { get; set; }
    public string? CompanyLogo { get; set; }
    public int TotalVehicles { get; set; }
    public ICollection<VehicleResponseList> Vehicles { get; set; } = new List<VehicleResponseList>();
    public ICollection<OfferResponseList> Offers { get; set; } = new List<OfferResponseList>();
    public ICollection<AwardsResponseList> Awards { get; set; } = new List<AwardsResponseList>();
    public ICollection<InvoiceResponseList> Invoices { get; set; } = new List<InvoiceResponseList>();
}