using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.Customer.Request;

public class CustomerLoginRequest
{
    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must contain exactly four digits.")]
    public string Pin { get; set; } = string.Empty;
}
