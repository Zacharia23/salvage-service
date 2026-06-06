using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.Customer.Request;

public class VerifyCodeRequest : IValidatableObject
{
    public Guid? VerificationId { get; set; }
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(6, MinimumLength = 6)]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Code must contain exactly six digits.")]
    public string Code { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!VerificationId.HasValue && string.IsNullOrWhiteSpace(Phone))
        {
            yield return new ValidationResult(
                "VerificationId or phone is required.",
                new[] { nameof(VerificationId), nameof(Phone) });
        }
    }
}
