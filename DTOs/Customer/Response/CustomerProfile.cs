using SalvageCore.Models;

namespace SalvageCore.DTOs.Customer.Response;

public class CustomerProfile
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string IdentityType { get; set; }
    public string CardNumber { get; set; }
    public string? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool AcceptedTerms { get; set; } = false;
    public bool AccountVerified { get; set; } = false;
    public string Region { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<CustomerBids> Bids { get; set; } = new List<CustomerBids>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}