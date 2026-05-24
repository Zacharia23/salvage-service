using SalvageCore.Enums;

namespace SalvageCore.DTOs.Customer.Response;

public class CustomerList
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string IdentityType { get; set; }
    public string CardNumber { get; set; }
    public GenderEnum? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public bool AcceptedTerms { get; set; }
    public bool AccountVerified { get; set; }
    public string Region { get; set; }
    public int Bids { get; set; }
    public int Awards { get; set; }
    public DateTime? LastLogin { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
}