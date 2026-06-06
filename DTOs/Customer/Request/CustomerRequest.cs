using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.Customer.Request;

public class CustomerRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Password must be a four-digit PIN.")]
    public string Password { get; set; } = string.Empty;
}
