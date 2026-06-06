using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.Customer.Request;

public class ResendVerificationRequest
{
    [Required]
    public Guid VerificationId { get; set; }
}
