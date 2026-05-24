using SalvageCore.Enums;

namespace SalvageCore.DTOs.Customer.Request;

public class CompleteRegistrationReq
{
    public Guid CustomerId { get; set; }
    public Guid IdentityTypeId { get; set; }
    public string CardNumber { get; set; }
    public GenderEnum Gender { get; set; }
    public string BirthDate { get; set; }
    public bool AcceptedTerms { get; set; }
    public Guid RegionId { get; set; }
    public string? TaxNumber { get; set; }
    public string? VNumber { get; set; }
}