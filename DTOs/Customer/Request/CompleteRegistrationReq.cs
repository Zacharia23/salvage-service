using System.ComponentModel.DataAnnotations;
using SalvageCore.Enums;

namespace SalvageCore.DTOs.Customer.Request;

public class CompleteRegistrationReq
{
    public Guid CustomerId { get; set; }

    [Required]
    public Guid IdentityTypeId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string CardNumber { get; set; }

    public GenderEnum Gender { get; set; }

    [Required]
    public string BirthDate { get; set; }

    public bool AcceptedTerms { get; set; }

    [Required]
    public Guid RegionId { get; set; }
    public string? TaxNumber { get; set; }
    public string? VNumber { get; set; }
}
